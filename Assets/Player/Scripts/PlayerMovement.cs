using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {
	#region States
	private PlayerMovementState Grounded;
	private PlayerMovementState Airborne;
	private PlayerMovementState Climbing;
	private PlayerMovementState currentState;
	#endregion

	[SerializeField] PlayerSettings playerSettings;
	[SerializeField] SpriteRenderer glideSpriteRender;
	[SerializeField] private Checkpoint checkpoint;

	private PlayerInput userInput;
	private PhysicsMaterial2D groundMaterial;
	private bool facingRight = true;
	private bool isDead = false;

	private bool gliding = false;
	private float glideBreakTime = 0f;

	private bool usedFirstJump = false;
	private bool usedDoubleJump = false;
	private float lastJumpTime = 0f;

	private float lastDashTime = 0f;

	private Tilemap foregroundTileMap;

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
	public delegate void PlayerEvent();
	public delegate void PlayerLandEvent(PhysicsMaterial2D material);

	public event PlayerLandEvent LandEvent;
	public event PlayerEvent jumpEvent;
	public event PlayerEvent dashEvent;
	public event PlayerEvent dieEvent;
	public event PlayerEvent glideEvent;
	public event PlayerEvent respawnEvent;
	#endregion

	private void Start ()
	{
		Animator = GetComponent<Animator>();
		RigidBody2d = GetComponent<Rigidbody2D>();
		userInput = GetComponent<PlayerInput>();

		foregroundTileMap = GameObject.FindWithTag("Foreground").GetComponent<Tilemap>();

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
		if (isDead) { return;  }

		currentState.Update();

		Animator.SetFloat("Speed", Mathf.Abs(userInput.HorizontalThrow));
		Animator.SetFloat("VerticalSpeed", RigidBody2d.velocity.y);

		if (FellOutOfWorld())
		{
			Die();
		}
	}

	private void Die()
	{
		if (dieEvent != null) { dieEvent(); }
		isDead = true;
		Animator.SetBool("Dead", isDead);

		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(playerSettings.RespawnTimeSeconds);
		if (respawnEvent != null) { respawnEvent(); }

		isDead = false;
		Animator.SetBool("Dead", isDead);
		Teleport(checkpoint.transform.position);
	}

	private bool FellOutOfWorld()
	{
		return (transform.position.y < playerSettings.WorldBottomThreshold);
	}

	private void Flip()
	{
		transform.localScale = new Vector3(transform.localScale.x * -1,
												transform.localScale.y,
												transform.localScale.z);
		
	}

	private float GetScaler()
	{
		return (facingRight)? 1 : -1;
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

	public TileBase GetSurfaceBeneath()
	{
		var downardVector = transform.up * .2f * -1;
		return foregroundTileMap.GetTile(Vector3Int.FloorToInt(playerSettings.GroundCheckPoint.position + downardVector));
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

		if (dashEvent != null) { dashEvent(); }

		RigidBody2d.AddForce(playerSettings.DashVector * GetScaler());
		lastDashTime = Time.time;
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
		if(glideEvent != null) { glideEvent(); }
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

		ResetConstraints();

		var scaler = (facingRight) ? 1 : -1;
		var appliedJumpForce = new Vector2(jumpForce.x * GetScaler(), jumpForce.y);
		Jump(appliedJumpForce);

	}
	public void Jump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }

		if (jumpEvent != null) { jumpEvent(); }

		usedFirstJump = true;
		lastJumpTime = Time.time;

		RigidBody2d.AddForce(jumpForce);
	}
	#endregion

	public void Teleport (Vector3 positionTo)
	{
		transform.position = positionTo;
	}

	public void SnapToWall()
	{
		var scaler = (facingRight) ? 1 : -1;
		var hit = Physics2D.Raycast(playerSettings.WallCheckPoint.position, Vector2.right * GetScaler(), playerSettings.WallSnapRaycastDistance, playerSettings.WhatIsWall);

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

	public void SetCheckPoint(Checkpoint checkpoint)
	{
		this.checkpoint = checkpoint;
	}

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
