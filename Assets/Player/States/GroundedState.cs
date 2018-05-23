using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerMovementState {
	private PlayerMovement playerMovement;
	private PlayerSettings PlyrSttings;

	private Vector2 dashVector = new Vector2(600, 0f);
	private float lastDashTime = 0f;
	private float dashCoolDownSeconds = 0.5f;

	public GroundedState (PlayerMovement playerMovement, PlayerSettings PlyrSttings)
	{
		this.playerMovement = playerMovement;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState() {}

	public void Update (float horizontalThrow, float verticalThrow, bool jump, bool dash, bool glide)
	{
		if( CheckGroundCollision() ) return;
		if( CheckWallGrip(horizontalThrow) ) return;

		Move(horizontalThrow, verticalThrow);

		if (dash && (Time.time - lastDashTime < dashCoolDownSeconds) )
			Dash();
		if(jump) Jump();
	}

	private bool CheckWallGrip(float horizontalThrow)
	{
		Collider2D[] wallColliders =
			Physics2D.OverlapCircleAll(PlyrSttings.WallCheckPoint.position, PlyrSttings.WallCheckRadius, PlyrSttings.WhatIsWall);

		if (wallColliders.Length > 0)
		{
			var facingRightAndGriping = playerMovement.FacingRight && horizontalThrow > PlyrSttings.WallGripThreshold;
			var facingLeftAndGriping = (!playerMovement.FacingRight && horizontalThrow < PlyrSttings.WallGripThreshold * -1);

			if ( facingRightAndGriping || facingLeftAndGriping)
			{
				playerMovement.TransitionGroundToClimb();
				return true;
			}
		}
		return false;
	}

	private bool CheckGroundCollision()
	{
		Collider2D[] groundColliders =
			Physics2D.OverlapCircleAll(PlyrSttings.GroundCheckPoint.position, PlyrSttings.GroundCheckRadius, PlyrSttings.WhatIsGround);

		if (groundColliders.Length == 0)
		{
			playerMovement.TransitionGroundToAir();
			return true;
		}
		return false;
	}

	private void Dash()
	{
		if (playerMovement.FacingRight)
		{
			playerMovement.RigidBody2d.AddForce(dashVector);
		}
		else
		{
			playerMovement.RigidBody2d.AddForce(dashVector * -1);
		}
		lastDashTime = Time.time;
	}

	private void Jump()
	{
		playerMovement.RigidBody2d.AddForce(PlyrSttings.GroundJumpForce);
		playerMovement.Jumping();
	}

	private void Move(float horizontalThrow, float verticalThrow)
	{
		var rigidBody2d = playerMovement.RigidBody2d;
		rigidBody2d.AddForce(new Vector2(horizontalThrow * PlyrSttings.MaxGroundSpeed * Time.deltaTime, 0f));

		CheckCharacterFlip(horizontalThrow);
	}

	private void CheckCharacterFlip(float horizontalThrow)
	{
		if (horizontalThrow < 0 && playerMovement.FacingRight)
		{
			playerMovement.FacingRight = false;
			playerMovement.Flip();
		}
		else if (horizontalThrow > 0 && !playerMovement.FacingRight)
		{
			playerMovement.FacingRight = true;
			playerMovement.Flip();
		}
	}
}
