using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : PlayerMovementState
{
	private PlayerController playerController;
	private PlayerLocomotion playerLocomotion;
	private PlayerSettings PlyrSttings;

	public ClimbingState(PlayerController playerController, PlayerLocomotion playerLocomotion, PlayerSettings PlyrSttings)
	{
		this.playerController = playerController;
		this.playerLocomotion = playerLocomotion;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState()
	{
		playerController.SetGravityScale(0f);
		playerController.SetBothConstraints();

		playerController.SnapToWall();
	}
	private void ExitState()
	{
		playerController.SetGravityToOriginal();
		playerController.ResetConstraints();

		if (playerController.IsTouchingGround())
		{
			playerController.TransitionClimbToGround();
		}
		else
		{
			playerController.TransitonClimbToAir();
		}
	}

	public void Update()
	{
		if( !playerController.IsGrippingWall())
		{
			ExitState();
		}

		playerLocomotion.WallClimb(PlyrSttings.WallClimbSpeed);
		playerLocomotion.WallJump(PlyrSttings.WallJumpForce);
	}
}
