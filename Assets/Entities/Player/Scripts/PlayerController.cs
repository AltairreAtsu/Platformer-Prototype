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
	#region Scriptable Objects
	[SerializeField] private PlayerSettings playerSettings;
	[SerializeField] private PlayerLocomotion playerLocomotion;
	#endregion
	[Space]
	#region Serialized Fields
	[SerializeField] private Transform attackCheckPoint;
	[SerializeField] private Checkpoint checkpoint;
	[Header ("Events")]
	[SerializeField] private GameEvent attackEvent;
	[SerializeField] private GameEvent footStepEvent;
	[SerializeField] private GameEvent climbEvent;
	[SerializeField] private GameLandEvent landEvent;
	#endregion

	#region Dependencies
	private IDamagable healthScript;
	private PlayerCollisionHandler collisionHandler;
	private PlayerInput userInput;
	private Tilemap foregroundTileMap;
	#endregion
	#region State Variables
	private PhysicsMaterial2D lastTouchedGroundMaterial;
	private float lastAttackTime;
	private bool facingRight = true;
	private bool isDead = false;
	#endregion
	#region Properties
	public Animator Animator
	{
		get;
		private set;
	}
	public Rigidbody2D RigidBody2d
	{
		get;
		private set;
	}

	public PlayerSettings PlayerSettings
	{
		get { return playerSettings; }
	}

	public bool FacingRight
	{
		get { return facingRight; }
		set { facingRight = value; }
	}
	#endregion

	#region StartUp Methods
	private void Start ()
	{
		FindDependencies();
		IntializeDependencyValues();
		InitializeStates();

		currentState = Grounded;
		TeleportToCheckpoint();
	}

	private void FindDependencies()
	{
		Animator = GetComponent<Animator>();
		RigidBody2d = GetComponent<Rigidbody2D>();
		userInput = GetComponent<PlayerInput>();
		collisionHandler = GetComponentInChildren<PlayerCollisionHandler>();
		foregroundTileMap = GameObject.FindWithTag("Foreground").GetComponent<Tilemap>();
		healthScript = GetComponent<IDamagable>();
	}

	private void IntializeDependencyValues()
	{
		playerSettings.GroundCheckPoint = transform.Find("GroundCheck");
		playerSettings.WallCheckPoint = transform.Find("WallCheck");
		RigidBody2d.gravityScale = playerSettings.DefaultGravityScale;
		collisionHandler.enemyCollisionEvent += TakeDamage;
	}

	private void InitializeStates()
	{
		Grounded = new GroundedState(this, playerLocomotion, playerSettings);
		Airborne = new AirborneState(this, playerLocomotion, playerSettings);
		Climbing = new ClimbingState(this, playerLocomotion, playerSettings);
	}
	#endregion

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

	#region World Interaction and Detection
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
			lastTouchedGroundMaterial = groundColliders[0].sharedMaterial;
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
	#endregion

	#region Direct Transform Modifiers
	public void Teleport (Vector3 positionTo)
	{
		transform.position = positionTo;
	}
	public void TeleportToCheckpoint()
	{
		transform.position = checkpoint.transform.position;
	}

	public void SetTransformParent(Transform newParent)
	{
		transform.SetParent(newParent, true);
	}
	public void UnSetTransformParent()
	{
		transform.SetParent(null);
	}

	public void SnapToWall()
	{
		var hit = Physics2D.Raycast(playerSettings.WallCheckPoint.position, Vector2.right * GetScaler(), playerSettings.WallSnapRaycastDistance, playerSettings.WhatIsWall);

		var distanceToWall = hit.distance;

		transform.position = (facingRight) ? 
			new Vector3(transform.position.x + distanceToWall, transform.position.y, transform.position.z) :
			new Vector3(transform.position.x - distanceToWall, transform.position.y, transform.position.z);

	}
	#endregion

	#region Combat Methods
	public void Attack()
	{
		if (!userInput.DoAttack || Time.time - lastAttackTime < playerSettings.AttackCooldownInSeconds) { return; }
		
		var colliders = Physics2D.OverlapBoxAll(attackCheckPoint.position, playerSettings.AttackDetectionBoxSize, 0f, playerSettings.WhatIsEnemy);
		attackEvent.Raise();
		if (colliders == null || colliders.Length == 0 ) { return; }
		
		for(int i = 0; i < colliders.Length; i++)
		{
			var damagableEntity = colliders[i].GetComponentInParent<IDamagable>();
		
			if (damagableEntity != null)
			{
				damagableEntity.Damage(playerSettings.BaseAttackDamage, transform);
			}
		}

		lastAttackTime = Time.time;
	}

	private void TakeDamage(float damage, Transform damageDealer)
	{
		healthScript.Damage(damage, damageDealer);
	}
	#endregion

	#region Audio Wrapper Methods
	public void RaiseFootStepEvent()
	{
		footStepEvent.Raise();
	}
	public void RaiseClimbEvent()
	{
		climbEvent.Raise();
	}
	#endregion

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

	public void SetCollidersActiveState(bool collidersActive)
	{
		var colliders = GetComponentsInChildren<Collider2D>();
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = collidersActive;
		}
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
		
		playerLocomotion.ResetJumps();
		if (playerLocomotion.Gliding) playerLocomotion.StopGliding();

		Animator.SetBool("AirBorne", false);
		landEvent.Raise(lastTouchedGroundMaterial);
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

	#region Utility Methods
	//TODO consider moving upstream to Entity Class
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

	public void SetCheckPoint(Checkpoint checkpoint)
	{
		this.checkpoint = checkpoint;
	}

	public float GetScaler()
	{
		return (facingRight) ? 1 : -1;
	}
	public float GetInvertedScaler()
	{
		return (facingRight) ? -1 : 1;
	}

	//TODO consider moving upstream to Entity Class
	private bool FellOutOfWorld()
	{
		return (transform.position.y < playerSettings.WorldBottomThreshold);
	}
	#endregion
}
