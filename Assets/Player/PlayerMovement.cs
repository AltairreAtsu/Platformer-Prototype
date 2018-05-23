using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	#region States
	private PlayerMovementState Grounded;
	private PlayerMovementState Airborne;
	private PlayerMovementState Climbing;
	private PlayerMovementState currentState;
	#endregion

	[SerializeField] PlayerSettings playerSettings;
	[SerializeField] SpriteRenderer glideSpriteRender;

	private PlayerInput userInput;
	private PhysicsMaterial2D groundMaterial;
	private bool facingRight = true;

	private bool gliding = false;
	private float glideBreakTime = 0f;

	private bool usedFirstJump = false;
	private bool usedDoubleJump = false;
	private float lastJumpTime = 0f;

	private float lastDashTime = 0f;

	#region Properties
	public Animator Animator { get; private set; }
	public Rigidbody2D RigidBody2d { get; private set; }
	public bool FacingRight
	{
		get { return facingRight; }
		set { facingRight = value; }
	}
	public bool Gliding
	{
		get { return gliding; }
	}
	#endregion

	#region Events
	public delegate void PlayerMovementEvent();
	public delegate void PlayerLandEvent(PhysicsMaterial2D material);

	public event PlayerLandEvent LandEvent;
	public event PlayerMovementEvent jumpEvent;
	public event PlayerMovementEvent dashEvent;
	#endregion

	private void Start ()
	{
		Animator = GetComponent<Animator>();
		RigidBody2d = GetComponent<Rigidbody2D>();
		userInput = GetComponent<PlayerInput>();

		Grounded = new GroundedState(this, playerSettings);
		Airborne = new AirborneState(this, playerSettings);
		Climbing = new ClimbingState(this, playerSettings);

		playerSettings.GroundCheckPoint = transform.Find("GroundCheck");
		playerSettings.WallCheckPoint = transform.Find("WallCheck");
		RigidBody2d.gravityScale = playerSettings.DefaultGravityScale;

		currentState = Grounded;
	}

	private void FixedUpdate ()
	{
		currentState.Update();

		Animator.SetFloat("Speed", Mathf.Abs(userInput.HorizontalThrow));
		Animator.SetFloat("VerticalSpeed", RigidBody2d.velocity.y);
	}

	private void Flip()
	{
		transform.localScale = new Vector3(transform.localScale.x * -1,
												transform.localScale.y,
												transform.localScale.z);
		
	}
	public void CheckCharacterFlip()
	{
		if (userInput.HorizontalThrow < 0 && facingRight)
		{
			facingRight = false;
			Flip();
		}
		else if (userInput.HorizontalThrow > 0 && !facingRight)
		{
			facingRight = true;
			Flip();
		}
	}

	public bool IsGrippingWall()
	{
		Collider2D[] wallColliders =
			Physics2D.OverlapCircleAll(playerSettings.WallCheckPoint.position, playerSettings.WallCheckRadius, playerSettings.WhatIsWall);

		if (wallColliders.Length > 0)
		{
			var facingRightAndGriping = facingRight && userInput.HorizontalThrow > playerSettings.WallGripThreshold;
			var facingLeftAndGriping = (!facingRight && userInput.HorizontalThrow < playerSettings.WallGripThreshold * -1);

			if (facingRightAndGriping || facingLeftAndGriping)
			{
				return true;
			}
		}
		return false;
	}
	public bool IsTouchingGround()
	{
		Collider2D[] groundColliders =
			Physics2D.OverlapCircleAll(playerSettings.GroundCheckPoint.position, playerSettings.GroundCheckRadius, playerSettings.WhatIsGround);

		if (groundColliders.Length > 0)
		{
			groundMaterial = groundColliders[0].sharedMaterial;
			return true;
		}
		return false;
	}

	#region Movement Methods
	public void MoveHorizontal(float speed)
	{
		RigidBody2d.AddForce(new Vector2(userInput.HorizontalThrow * speed * Time.deltaTime, 0f));
	}
	public void WallClimb(float speed)
	{
		RigidBody2d.AddForce(new Vector2(0f, userInput.VerticalThrow * speed * Time.deltaTime));
	}

	public void Dash()
	{
		var dashIOnCooldown = !(Time.time - lastDashTime > playerSettings.DashCooldownSeconds);
		if (!userInput.DoDash || dashIOnCooldown) { return; }

		if (facingRight)
		{
			RigidBody2d.AddForce(playerSettings.DashVector);
		}
		else
		{
			RigidBody2d.AddForce(playerSettings.DashVector * -1);
		}
		lastDashTime = Time.time;

		if (dashEvent != null) { dashEvent(); }
	}

	public void UpdateGlide()
	{
		var glideIsOnCooldown = Time.time - glideBreakTime < playerSettings.GlideBreakCoolDownSeconds;

		if (userInput.DoGlide && !gliding && !glideIsOnCooldown)
		{
			StartGliding();
		}
		else if (!userInput.DoGlide && gliding)
		{
			StopGliding();
		}
	}
	private void StartGliding()
	{
		// TODO seperate glide Visuals from glide logic
		gliding = true;
		SetGravityScale(playerSettings.GlidingGravity);
		glideSpriteRender.enabled = true;
	}
	private void StopGliding()
	{
		gliding = false;
		SetGravityToOriginal();
		glideSpriteRender.enabled = false;
		glideBreakTime = Time.time;
	}

	public void AirJump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }

		var jumpIsOnCoolDown = Time.time - lastJumpTime < playerSettings.JumpCooldownSeconds;
		if (usedFirstJump && usedDoubleJump || jumpIsOnCoolDown) { return; }

		else if (!usedFirstJump && !usedDoubleJump)
		{
			usedFirstJump = true;
		}
		else if (usedFirstJump && !usedDoubleJump)
		{
			usedDoubleJump = true;
		}
		if (gliding)
		{
			StopGliding();
		}

		RigidBody2d.velocity = new Vector2(RigidBody2d.velocity.x, 0f);
		Jump(jumpForce);
	}
	public void WallJump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }
		var scaler = (facingRight) ? 1 : -1;

		ResetConstraints();
		var appliedJumpForce = new Vector2(jumpForce.x * scaler, jumpForce.y);
		Jump(appliedJumpForce);

	}
	public void Jump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }

		RigidBody2d.AddForce(jumpForce);
		usedFirstJump = true;
		lastJumpTime = Time.time;
		if (jumpEvent != null) { jumpEvent(); }
	}
	#endregion

	public void Teleport (Vector3 positionTo)
	{
		transform.position = positionTo;
	}
	public void SnapToWall()
	{
		var scaler = (facingRight) ? 1 : -1;
		var hit = Physics2D.Raycast(playerSettings.WallCheckPoint.position, Vector2.right * scaler, playerSettings.WallSnapRaycastDistance, playerSettings.WhatIsWall);

		var distanceToWall = hit.distance;

		transform.position = (facingRight) ? 
			new Vector3(transform.position.x + distanceToWall, transform.position.y, transform.position.z) :
			new Vector3(transform.position.x - distanceToWall, transform.position.y, transform.position.z);

	}

	#region Physics Functions
	public void SetBothConstraints()
	{
		RigidBody2d.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		return;
	}
	public void ResetConstraints()
	{
		RigidBody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
	}
	public void SetGravityScale(float gravityScale)
	{
		RigidBody2d.gravityScale = gravityScale;
	}
	public void SetGravityToOriginal()
	{
		RigidBody2d.gravityScale = playerSettings.DefaultGravityScale;
	}
	#endregion

	#region Transitions
	public void TransitionGroundToAir()
	{
		currentState = Airborne;
		currentState.EnterState();
		Animator.SetBool("AirBorne", true);
	}
	public void TransitionAirToGround()
	{
		currentState = Grounded;
		Animator.SetBool("AirBorne", false);

		usedFirstJump = false;
		usedDoubleJump = false;
		if (gliding) StopGliding();

		if (LandEvent != null) { LandEvent(groundMaterial); }
	}
	public void TransitionGroundToClimb()
	{
		currentState = Climbing;
		currentState.EnterState();
	}
	public void TransitionAirToClimb()
	{
		currentState = Climbing;
		currentState.EnterState();

		usedFirstJump = false;
		usedDoubleJump = false;
		if (gliding) StopGliding();

		Animator.SetBool("AirBorne", false);
	}
	public void TransitionClimbToGround()
	{
		currentState = Grounded;

	}
	public void TransitonClimbToAir()
	{
		currentState = Airborne;
		currentState.EnterState();
		Animator.SetBool("AirBorne", true);
	}
	#endregion
}
