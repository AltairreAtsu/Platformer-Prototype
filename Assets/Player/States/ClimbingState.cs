using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : PlayerMovementState
{
	private PlayerMovement player;
	private PlayerSettings PlyrSttings;

	public ClimbingState(PlayerMovement playerMovement, PlayerSettings PlyrSttings)
	{
		this.player = playerMovement;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState()
	{
		player.SetGravityScale(0f);
		player.SetBothConstraints();

		player.SnapToWall();
	}
	private void ExitState()
	{
		player.SetGravityToOriginal();
		player.ResetConstraints();

		if (player.IsTouchingGround())
		{
			player.TransitionClimbToGround();
		}
		else
		{
			player.TransitonClimbToAir();
		}
	}

	public void Update()
	{
		if( !player.IsGrippingWall())
		{
			ExitState();
		}

		player.WallClimb(PlyrSttings.WallClimbSpeed);
		player.WallJump(PlyrSttings.WallJumpForce);
	}
}
