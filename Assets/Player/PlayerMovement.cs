﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	private PlayerMovementState Grounded;
	private PlayerMovementState Airborne;
	private PlayerMovementState Climbing;
	private PlayerMovementState currentState;

	[SerializeField] PlayerSettings playerSettings;
	[SerializeField] AirDash airDash;
	[SerializeField] SpriteRenderer glideSpriteRender;

	private PlayerInput userInput;
	private bool facingRight = true;

	public Animator Animator { get; private set; }
	public Rigidbody2D RigidBody2d { get; private set; }
	public bool FacingRight
	{
		get { return facingRight; }
		set { facingRight = value; }
	}

	private void Start ()
	{
		Animator = GetComponent<Animator>();
		RigidBody2d = GetComponent<Rigidbody2D>();
		userInput = GetComponent<PlayerInput>();

		Grounded = new GroundedState(this, playerSettings);
		Airborne = new AirborneState(this, playerSettings);
		Climbing = new ClimbingState(this, playerSettings);

		playerSettings.GroundCheckPoint = transform.Find("GroundCheck");
		playerSettings.WallCheckPoint = transform.Find("WallCheck");
		RigidBody2d.gravityScale = playerSettings.DefaultGravityScale;

		currentState = Grounded;
	}

	private void FixedUpdate ()
	{
		currentState.Update(userInput.HorizontalThrow, userInput.VerticalThrow, userInput.DoJump, userInput.DoDash, userInput.DoGlide);

		Animator.SetFloat("Speed", Mathf.Abs(userInput.HorizontalThrow));
		Animator.SetFloat("VerticalSpeed", RigidBody2d.velocity.y);
	}

	public void Flip()
	{
		transform.localScale = new Vector3(transform.localScale.x * -1,
												transform.localScale.y,
												transform.localScale.z);
		
	}

	public void AirDash()
	{
		airDash.StartCoroutine(airDash.DoDash());
	}
	public void Glide(bool glide)
	{
		glideSpriteRender.enabled = glide;
	}

	public void TransitionGroundToAir()
	{
		currentState = Airborne;
		currentState.EnterState();
		Animator.SetBool("AirBorne", true);
	}
	public void TransitionAirToGround()
	{
		currentState = Grounded;
		Animator.SetBool("AirBorne", false);
	}
	public void TransitionGroundToClimb()
	{
		currentState = Climbing;
		currentState.EnterState();
	}
	public void TransitionAirToClimb()
	{
		currentState = Climbing;
		currentState.EnterState();
		Animator.SetBool("AirBorne", false);
	}
	public void TransitionClimbToGround()
	{
		currentState = Grounded;

	}
	public void TransitionClimbToAir()
	{
		currentState = Airborne;
		currentState.EnterState();
		Animator.SetBool("AirBorne", true);
	}
}
