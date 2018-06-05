using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput : MonoBehaviour {
	#region Internal variables
	private float horizontalThrow = 0f;
	private float verticalThrow = 0f;

	private bool jump = false;
	private bool dash = false;
	private bool glide = false;
	private bool attack = false;

	private bool hasControl = true;
	#endregion
	#region Properties
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
	public bool DoAttack
	{
		get { return attack; }
	}
	#endregion

	private void Update()
	{
		if (!hasControl) { return; }
		jump = CrossPlatformInputManager.GetButtonDown("Jump");
		dash = CrossPlatformInputManager.GetButtonDown("Dash");
		glide = CrossPlatformInputManager.GetButton("Glide");
		attack = CrossPlatformInputManager.GetButtonDown("Fire1");
	}

	private void FixedUpdate ()
	{
		if (!hasControl) { return; }
		horizontalThrow = CrossPlatformInputManager.GetAxis("Horizontal");
		verticalThrow = CrossPlatformInputManager.GetAxis("Vertical");
	}

	public void SetHasControl(bool hasControl)
	{
		if (!hasControl)
		{
			horizontalThrow = 0;
			verticalThrow = 0;
		}

		this.hasControl = hasControl;
	}
	public bool HasControl()
	{
		return hasControl;
	}

}
