using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {
	#region States
	private PlayerMovementState Grounded;
	private PlayerMovementState Airborne;
	private PlayerMovementState Climbing;
	private PlayerMovementState currentState;
	#endregion

	[SerializeField] private PlayerSettings playerSettings;
	[SerializeField] private Checkpoint checkpoint;

	[SerializeField] private Transform attackCheckPoint;
	[SerializeField] private Vector2 attackDetectionBoxSize;
	[SerializeField] private LayerMask whatIsEnemy;
	[SerializeField] private float damageToDeal;
	[SerializeField] private float attackCooldownInSeconds;

	private float lastAttackTime;

	private IDamagable healthScript;
	private PlayerCollisionHandler collisionHandler;
	private PlayerInput userInput;
	private PlayerLocomotion playerLocomotion;
	private PhysicsMaterial2D groundMaterial;
	private bool facingRight = true;
	private bool isDead = false;

	private Tilemap foregroundTileMap;

	#region Properties
	public Animator Animator { get; private set; }
	public Rigidbody2D RigidBody2d { get; private set; }
	public bool FacingRight
	{
		get { return facingRight; }
		set { facingRight = value; }
	}
	public PlayerSettings PlayerSettings { get { return playerSettings; } }
	#endregion

	public delegate void PlayerEvent();
	public delegate void PlayerLandEvent(PhysicsMaterial2D material);

	public event PlayerEvent attackEvent;
	public event PlayerLandEvent LandEvent;

	private void Start ()
	{
		Animator = GetComponent<Animator>();
		RigidBody2d = GetComponent<Rigidbody2D>();
		playerLocomotion = GetComponent<PlayerLocomotion>();
		userInput = GetComponent<PlayerInput>();

		collisionHandler = GetComponentInChildren<PlayerCollisionHandler>();
		collisionHandler.enemyCollisionEvent += TakeDamage;

		healthScript = GetComponent<IDamagable>();

		foregroundTileMap = GameObject.FindWithTag("Foreground").GetComponent<Tilemap>();

		Grounded = new GroundedState(this, playerLocomotion, playerSettings);
		Airborne = new AirborneState(this, playerLocomotion, playerSettings);
		Climbing = new ClimbingState(this, playerLocomotion, playerSettings);

		playerSettings.GroundCheckPoint = transform.Find("GroundCheck");
		playerSettings.WallCheckPoint = transform.Find("WallCheck");
		RigidBody2d.gravityScale = playerSettings.DefaultGravityScale;

		currentState = Grounded;
	}

	private void FixedUpdate ()
	{
		if (isDead) { return;  }

		currentState.Update();

		Animator.SetFloat("Absolute Horizontal Control Throw", Mathf.Abs(userInput.HorizontalThrow));
		Animator.SetFloat("Vertical Velocity", RigidBody2d.velocity.y);
		Animator.SetFloat("Absolute Vertical Control Throw", Mathf.Abs(userInput.VerticalThrow));

		if (FellOutOfWorld())
		{
			healthScript.Die();
		}
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

	public float GetScaler()
	{
		return (facingRight)? 1 : -1;
	}
	public float GetInvertedScaler()
	{
		return (facingRight) ? -1 : 1;
	}

	public bool IsGrippingWall()
	{
		if(Time.time - playerLocomotion.ClimbBreakTime < playerSettings.WallBreakCooldown) { return false;  }

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

	public TileBase GetTileSurfaceBeneath()
	{
		var downardVector = transform.up * .2f * -1;
		return foregroundTileMap.GetTile(Vector3Int.FloorToInt(playerSettings.GroundCheckPoint.position + downardVector));
	}
	public TileBase GetTileSurfaceClimbing()
	{
		var facingVector = transform.right * .2f;
		return foregroundTileMap.GetTile(Vector3Int.FloorToInt(playerSettings.WallCheckPoint.position + facingVector));
	}

	public void Teleport (Vector3 positionTo)
	{
		transform.position = positionTo;
	}
	public void TeleportToCheckpoint()
	{
		transform.position = checkpoint.transform.position;
	}

	public void SnapToWall()
	{
		var hit = Physics2D.Raycast(playerSettings.WallCheckPoint.position, Vector2.right * GetScaler(), playerSettings.WallSnapRaycastDistance, playerSettings.WhatIsWall);

		var distanceToWall = hit.distance;

		transform.position = (facingRight) ? 
			new Vector3(transform.position.x + distanceToWall, transform.position.y, transform.position.z) :
			new Vector3(transform.position.x - distanceToWall, transform.position.y, transform.position.z);

	}

	public void Attack()
	{
		if (!userInput.DoAttack || Time.time - lastAttackTime < attackCooldownInSeconds) { return; }
		
		var colliders = Physics2D.OverlapBoxAll(attackCheckPoint.position, attackDetectionBoxSize, 0f, whatIsEnemy);
		if (attackEvent != null) { attackEvent(); }
		if (colliders == null || colliders.Length == 0 ) { return; }
		
		for(int i = 0; i < colliders.Length; i++)
		{
			var damagableEntity = colliders[i].GetComponentInParent<IDamagable>();
		
			if (damagableEntity != null)
			{
				damagableEntity.Damage(damageToDeal, transform);
			}
		}

		lastAttackTime = Time.time;
	}

	private void TakeDamage(float damage, Transform damageDealer)
	{
		healthScript.Damage(damage, damageDealer);
	}

	public void SetCollidersActiveState(bool collidersActive)
	{
		var colliders = GetComponentsInChildren<Collider2D>();
		for(int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = collidersActive;
		}
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
		
		playerLocomotion.ResetJumps();
		if (playerLocomotion.Gliding) playerLocomotion.StopGliding();

		Animator.SetBool("AirBorne", false);
		if (LandEvent != null) { LandEvent(groundMaterial); }
	}
	public void TransitionGroundToClimb()
	{
		currentState = Climbing;
		currentState.EnterState();
		Animator.SetBool("Climbing", true);
	}
	public void TransitionAirToClimb()
	{
		currentState = Climbing;
		currentState.EnterState();

		playerLocomotion.ResetJumps();
		if (playerLocomotion.Gliding) playerLocomotion.StopGliding();

		Animator.SetBool("AirBorne", false);
		Animator.SetBool("Climbing", true);
	}
	public void TransitionClimbToGround()
	{
		currentState = Grounded;
		Animator.SetBool("Climbing", false);
	}
	public void TransitonClimbToAir()
	{
		currentState = Airborne;
		currentState.EnterState();
		Animator.SetBool("AirBorne", true);
		Animator.SetBool("Climbing", false);
	}
	#endregion
}
