using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput : MonoBehaviour {
	private float horizontalThrow = 0f;
	private float verticalThrow = 0f;

	private bool jump = false;
	private bool dash = false;
	private bool glide = false;

	public float HorizontalThrow
	{
		get { return horizontalThrow; }
	}
	public float VerticalThrow
	{
		get { return verticalThrow; }
	}
	public bool DoJump
	{
		get { return jump; }
	}
	public bool DoDash
	{
		get { return dash; }
	}
	public bool DoGlide
	{
		get { return glide; }
	}

	private void Update()
	{
		jump = CrossPlatformInputManager.GetButtonDown("Jump");
		dash = CrossPlatformInputManager.GetButtonDown("Dash");
		glide = CrossPlatformInputManager.GetButton("Glide");
	}

	void FixedUpdate () {
		horizontalThrow = CrossPlatformInputManager.GetAxis("Horizontal");
		verticalThrow = CrossPlatformInputManager.GetAxis("Vertical");
	}
}
