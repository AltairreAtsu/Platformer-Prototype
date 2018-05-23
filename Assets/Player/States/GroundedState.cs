using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerMovementState {
	private PlayerMovement player;
	private PlayerSettings PlyrSttings;

	public GroundedState (PlayerMovement playerMovement, PlayerSettings PlyrSttings)
	{
		player = playerMovement;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState() {}

	public void Update ()
	{
		if( !player.IsTouchingGround() )
		{
			player.TransitionGroundToAir();
			return;
		}
		if (player.IsGrippingWall())
		{
			player.TransitionGroundToClimb();
			return;
		}

		player.MoveHorizontal(PlyrSttings.GroundSpeed);
		player.CheckCharacterFlip();
		player.Dash();
		player.Jump(PlyrSttings.GroundJumpForce);
	}
}
