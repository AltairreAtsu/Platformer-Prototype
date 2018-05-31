using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Player Locomotion", menuName ="Platformer/PlayerController/PlayerLocomotion")]
public class PlayerLocomotion : MonoBehaviour {
	#region Injected Dependencies
	[Header ("Dependencies")]
	[SerializeField] private PlayerController playerController;
	[SerializeField] private PlayerSettings playerSettings;
	[SerializeField] private PlayerInput userInput;
	[Header("Events")]
	[SerializeField] private GameEvent jumpEvent;
	[SerializeField] private GameEvent dashEvent;
	[SerializeField] private GameEvent startGlideEvent;
	[SerializeField] private GameEvent stopGlideEvent;

	private Rigidbody2D rigidbody2d;

	#endregion
	#region State Variables
	private bool gliding = false;
	private float glideBreakTime = 0f;

	private bool usedFirstJump = false;
	private bool usedDoubleJump = false;
	private float lastJumpTime = 0f;

	private float climbBreakTime = 0f;

	private float lastDashTime = 0f;
	#endregion
	#region Properties
	public float ClimbBreakTime
	{
		get { return climbBreakTime; }
	}
	public bool Gliding
	{
		get { return gliding; }
	}
	#endregion

	private void Start()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();
		lastDashTime = 0;
		lastJumpTime = 0;
		climbBreakTime = 0;
		glideBreakTime = 0;
		ResetJumps();
		gliding = false;
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
	#endregion

	public void Dash()
	{
		var dashIOnCooldown = !(Time.time - lastDashTime > playerSettings.DashCooldownSeconds);
		if (!userInput.DoDash || dashIOnCooldown) { return; }

		dashEvent.Raise();

		rigidbody2d.AddForce(playerSettings.DashVector * playerController.GetScaler());
		lastDashTime = Time.time;
	}
	
	#region Glide Methods
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
		startGlideEvent.Raise();
		gliding = true;
		playerController.SetGravityScale(playerSettings.GlidingGravity);
	}

	public void StopGliding()
	{
		gliding = false;
		playerController.SetGravityToOriginal();
		stopGlideEvent.Raise();
		glideBreakTime = Time.time;
	}
	#endregion

	#region Jump Methods
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

		var appliedJumpForce = new Vector2(jumpForce.x * (playerController.GetInvertedScaler()), jumpForce.y);
		Jump(appliedJumpForce);
	}

	public void Jump(Vector2 jumpForce)
	{
		if (!userInput.DoJump) { return; }

		jumpEvent.Raise();

		usedFirstJump = true;
		lastJumpTime = Time.time;

		rigidbody2d.AddForce(jumpForce);
	}

	public void ResetJumps()
	{
		usedFirstJump = false;
		usedDoubleJump = false;
	}
#endregion
}
