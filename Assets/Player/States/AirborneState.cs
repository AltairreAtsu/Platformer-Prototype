using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerMovementState {
	private PlayerMovement player;
	private PlayerSettings PlyrSttings;

	public AirborneState(PlayerMovement playerMovement, PlayerSettings PlyrSttings)
	{
		this.player = playerMovement;
		this.PlyrSttings = PlyrSttings;
	}

	public void EnterState() {}

	public void Update()
	{
		if ( player.IsTouchingGround() )
		{
			player.TransitionAirToGround();
			return;
		}
		if ( player.IsGrippingWall() )
		{
			player.TransitionAirToClimb();
			return;
		}

		player.MoveHorizontal(PlyrSttings.AirSpeed);
		player.CheckCharacterFlip();
		player.UpdateGlide();
		player.AirJump(PlyrSttings.AirJumpForce);	
		player.Dash();
	}
}
