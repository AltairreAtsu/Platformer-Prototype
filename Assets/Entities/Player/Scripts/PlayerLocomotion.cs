using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

	[SerializeField] private SpriteRenderer glideSpriteRender;

	private Rigidbody2D rigidbody2d;

	private PlayerController playerController;
	private PlayerSettings playerSettings;
	private PlayerInput userInput;

	private bool gliding = false;
	private float glideBreakTime = 0f;

	private bool usedFirstJump = false;
	private bool usedDoubleJump = false;
	private float lastJumpTime = 0f;

	private float climbBreakTime = 0f;

	private float lastDashTime = 0f;

	public float ClimbBreakTime
	{
		get { return climbBreakTime; }
	}
	public bool Gliding
	{
		get { return gliding; }
	}

	#region Events
	public delegate void PlayerEvent();
	
	public event PlayerEvent jumpEvent;
	public event PlayerEvent dashEvent;
	public event PlayerEvent glideEvent;

	#endregion

	private void Start()
	{
		playerController = GetComponent<PlayerController>();
		userInput = GetComponent<PlayerInput>();
		rigidbody2d = GetComponent<Rigidbody2D>();
		playerSettings = playerController.PlayerSettings;
	}

	#region Movement Methods
	public void MoveHorizontal(float speed)
	{
		rigidbody2d.AddForce(new Vector2(userInput.HorizontalThrow * speed * Time.deltaTime, 0f));
	}
	public void WallClimb(float speed)
	{
		rigidbody2d.AddForce(new Vector2(0f, userInput.VerticalThrow * speed * Time.deltaTime));
	}

	public void Dash()
	{
		var dashIOnCooldown = !(Time.time - lastDashTime > playerSettings.DashCooldownSeconds);
		if (!userInput.DoDash || dashIOnCooldown) { return; }

		if (dashEvent != null) { dashEvent(); }

		rigidbody2d.AddForce(playerSettings.DashVector * playerController.GetScaler());
		lastDashTime = Time.time;
	}

	public void UpdateGlide()
	{
		var glideIsOnCooldown = Time.time - glideBreakTime < playerSettings.GlideBreakCoolDownSeconds;

		if (userInput.DoGlide && !gliding && !glideIsOnCooldown)
		{
			StartGliding();
		}
		else if (!userInput.DoGlide && gliding)
		{
			StopGliding();
		}
	}
	public void StartGliding()
	{
		// TODO seperate glide Visuals from glide logic
		if (glideEvent != null) { glideEvent(); }
		gliding = true;
		playerController.SetGravityScale(playerSettings.GlidingGravity);
		glideSpriteRender.enabled = true;
	}
	public void StopGliding()
	{
		gliding = false;
		playerController.SetGravityToOriginal();
		glideSpriteRender.enabled = false;
		glideBreakTime = Time.time;
	}

	public void AirJump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }

		var jumpIsOnCoolDown = Time.time - lastJumpTime < playerSettings.JumpCooldownSeconds;

		if (usedFirstJump && usedDoubleJump || jumpIsOnCoolDown) { return; }

		else if (!usedFirstJump && !usedDoubleJump)
		{
			usedFirstJump = true;
		}
		else if (usedFirstJump && !usedDoubleJump)
		{
			usedDoubleJump = true;
		}
		if (gliding)
		{
			StopGliding();
		}

		rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);
		Jump(jumpForce);
	}
	public void WallJump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }

		climbBreakTime = Time.time;

		playerController.ResetConstraints();
		playerController.SetGravityToOriginal();

		// Inverts Scaler to apply force in opposite direction
		var appliedJumpForce = new Vector2(jumpForce.x * (playerController.GetInvertedScaler()), jumpForce.y);
		Jump(appliedJumpForce);
	}
	public void Jump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }

		if (jumpEvent != null) { jumpEvent(); }

		usedFirstJump = true;
		lastJumpTime = Time.time;

		rigidbody2d.AddForce(jumpForce);
	}
	#endregion

	public void ResetJumps()
	{
		usedFirstJump = false;
		usedDoubleJump = false;
	}
}
