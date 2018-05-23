using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerMovementState {
	private PlayerMovement playerMovement;
	private PlayerSettings PlyrSttings;

	private float enterStateTime = 0f;
	private bool hasDoubleJump = true;

	private bool gliding = false;



	public AirborneState(PlayerMovement playerMovement, PlayerSettings PlyrSttings)
	{
		this.playerMovement = playerMovement;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState()
	{
		enterStateTime = Time.time;
	}

	public void Update(float horizontalThrow, float verticalThrow, bool jump, bool dash, bool glide)
	{
		if (CheckGroundCollision()) return;
		if (CheckWallGrip(horizontalThrow)) return;

		Move(horizontalThrow, verticalThrow);

		var doubleJumpAvailable = Time.time - enterStateTime > PlyrSttings.DoubleJumpDelaySeconds;

		CheckGlide(glide);

		if (gliding) { return; }

		if (jump && hasDoubleJump && doubleJumpAvailable)
			Jump();

		if (dash && Time.time - PlyrSttings.LastDashTime > PlyrSttings.DashCooldownSeconds)
			Dash();
	}


	private bool CheckWallGrip(float horizontalThrow)
	{
		Collider2D[] wallColliders =
			Physics2D.OverlapCircleAll(PlyrSttings.WallCheckPoint.position, PlyrSttings.WallCheckRadius, PlyrSttings.WhatIsWall);

		if (wallColliders.Length > 0)
		{
			var facingRightAndGripping = playerMovement.FacingRight && horizontalThrow > PlyrSttings.WallGripThreshold;
			var facingLeftAndGripping = (!playerMovement.FacingRight && horizontalThrow < PlyrSttings.WallGripThreshold * -1);

			if (facingRightAndGripping || facingLeftAndGripping)
			{
				hasDoubleJump = true;
				if (gliding) StopGliding();
				playerMovement.TransitionAirToClimb();
				return true;
			}
		}
		return false;
	}

	private bool CheckGroundCollision()
	{
		Collider2D[] colliders =
			Physics2D.OverlapCircleAll(PlyrSttings.GroundCheckPoint.position, PlyrSttings.GroundCheckRadius, PlyrSttings.WhatIsGround);

		if (colliders.Length > 0)
		{
			hasDoubleJump = true;
			if(gliding) StopGliding();
			// TODO Give Materials Priorities over other Materials
			playerMovement.TransitionAirToGround(colliders[0].sharedMaterial);
			return true;
		}
		return false;
	}

	private void Jump()
	{
		hasDoubleJump = false;
		var rigidBody2d = playerMovement.RigidBody2d;

		rigidBody2d.velocity = new Vector2(rigidBody2d.velocity.x, 0f);
		rigidBody2d.AddForce(PlyrSttings.DoubleJumpForce);
		playerMovement.Jumping();
	}

	private void Dash()
	{
		if (playerMovement.FacingRight)
		{
			playerMovement.RigidBody2d.AddForce(PlyrSttings.DashVector);
		}
		else
		{
			playerMovement.RigidBody2d.AddForce(PlyrSttings.DashVector * -1);
		}
		PlyrSttings.LastDashTime = Time.time;
		playerMovement.Dash();
	}

	private void CheckGlide(bool glide)
	{
		if (glide && !gliding)
		{
			StartGliding();
		}
		else if (!glide && gliding)
		{
			StopGliding();
		}
	}

	private void StartGliding()
	{
		gliding = true;
		playerMovement.RigidBody2d.gravityScale = PlyrSttings.GlidingGravity;
		playerMovement.Glide(gliding);
	}

	private void StopGliding()
	{
		gliding = false;
		playerMovement.RigidBody2d.gravityScale = PlyrSttings.DefaultGravityScale;
		playerMovement.Glide(gliding);
	}

	private void Move(float horizontalThrow, float verticalThrow)
	{
		var rigidBody2d = playerMovement.RigidBody2d;
		rigidBody2d.AddForce(new Vector2(horizontalThrow * PlyrSttings.MaxAirSpeed * Time.deltaTime, 0f));

		CheckFlip(horizontalThrow);
	}

	private void CheckFlip(float horizontalThrow)
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
