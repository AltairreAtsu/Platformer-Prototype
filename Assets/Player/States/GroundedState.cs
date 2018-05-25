using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerMovementState {
	private PlayerController playerController;
	private PlayerLocomotion playerLocomotion;
	private PlayerSettings PlyrSttings;

	public GroundedState (PlayerController playerController, PlayerLocomotion playerLocomotion, PlayerSettings PlyrSttings)
	{
		this.playerController = playerController;
		this.playerLocomotion = playerLocomotion;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState() {}

	public void Update ()
	{
		if( !playerController.IsTouchingGround() )
		{
			playerController.TransitionGroundToAir();
			return;
		}
		if (playerController.IsGrippingWall())
		{
			playerController.TransitionGroundToClimb();
			return;
		}

		playerLocomotion.MoveHorizontal(PlyrSttings.GroundSpeed);
		playerController.CheckCharacterFlip();
		playerLocomotion.Dash();
		playerLocomotion.Jump(PlyrSttings.GroundJumpForce);
	}
}
