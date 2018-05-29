using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerMovementState {
	private PlayerController playerController;
	private PlayerLocomotion playerLocomotion;
	private PlayerSettings PlyrSttings;

	public AirborneState(PlayerController playerController, PlayerLocomotion playerLocomotion, PlayerSettings PlyrSttings)
	{
		this.playerController = playerController;
		this.playerLocomotion = playerLocomotion;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState() {}

	public void Update()
	{
		if ( playerController.IsTouchingGround() )
		{
			playerController.TransitionAirToGround();
			return;
		}
		if ( playerController.IsGrippingWall() )
		{
			playerController.TransitionAirToClimb();
			return;
		}

		playerLocomotion.MoveHorizontal(PlyrSttings.AirSpeed);
		playerController.CheckCharacterFlip();
		playerLocomotion.UpdateGlide();
		playerLocomotion.AirJump(PlyrSttings.AirJumpForce);
		playerLocomotion.Dash();
	}
}
