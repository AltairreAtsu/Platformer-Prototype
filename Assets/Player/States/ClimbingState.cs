using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : PlayerMovementState
{
	private PlayerMovement playerMovement;
	private PlayerSettings PlyrSttings;

	private bool touchingGround = false;

	public ClimbingState(PlayerMovement playerMovement, PlayerSettings PlyrSttings)
	{
		this.playerMovement = playerMovement;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState()
	{
		playerMovement.RigidBody2d.gravityScale = 0;
	}

	public void Update(float horizontalThrow, float verticalThrow, bool jump, bool dash, bool glide)
	{
		touchingGround = false;

		CheckGroundCollision();
		if( CheckWallCollision(horizontalThrow) ) return;

		Move(horizontalThrow, verticalThrow);
		if (jump)
			Jump();
	}

	private bool CheckWallCollision(float horizontalThrow)
	{
		Collider2D[] wallColliders =
			Physics2D.OverlapCircleAll(PlyrSttings.WallCheckPoint.position, PlyrSttings.WallCheckRadius, PlyrSttings.WhatIsWall);

		if (wallColliders.Length > 0)
		{
			return CheckIsGrippingWall(horizontalThrow);
		}
		else if (wallColliders.Length == 0)
		{
			TransitionOutOfState();
			return true;
		}
		return false;
	}

	private bool CheckIsGrippingWall(float horizontalThrow)
	{
		var facingRightAndGripping = (playerMovement.FacingRight && horizontalThrow > PlyrSttings.WallGripThreshold);
		var facingLeftAndGriping = (!playerMovement.FacingRight && horizontalThrow < PlyrSttings.WallGripThreshold * -1);

		if (!facingRightAndGripping && !facingLeftAndGriping)
		{
			TransitionOutOfState();
			return true;
		}
		return false;
	}

	private void CheckGroundCollision()
	{
		Collider2D[] groundColliders =
			Physics2D.OverlapCircleAll(PlyrSttings.GroundCheckPoint.position, PlyrSttings.GroundCheckRadius, PlyrSttings.WhatIsGround);

		if (groundColliders.Length < 0)
		{
			Debug.Log("Touching Ground!");
			touchingGround = true;
		}
	}

	private void TransitionOutOfState()
	{
		if (touchingGround)
		{
			playerMovement.RigidBody2d.gravityScale = PlyrSttings.DefaultGravityScale;
			playerMovement.TransitionClimbToGround();
		}
		else
		{
			playerMovement.RigidBody2d.gravityScale = PlyrSttings.DefaultGravityScale;
			playerMovement.TransitionClimbToAir();
		}
	}

	public void Move(float horizontalThrow, float verticalThrow)
	{
		var rigidBody2d = playerMovement.RigidBody2d;
		//rigidBody2d.velocity = new Vector2(0f, verticalThrow * PlyrSttings.WallClimbSpeed * Time.deltaTime);


		rigidBody2d.AddForce(new Vector2(0f, verticalThrow * PlyrSttings.WallClimbSpeed * Time.deltaTime));
	}

	private void Jump ()
	{
		if (playerMovement.FacingRight)
		{	
			playerMovement.RigidBody2d.AddForce(new Vector2(PlyrSttings.WallJumpForce.x * -1, PlyrSttings.WallJumpForce.y));
		}
		else
		{
			playerMovement.RigidBody2d.AddForce(PlyrSttings.WallJumpForce);
		}
	}
}
