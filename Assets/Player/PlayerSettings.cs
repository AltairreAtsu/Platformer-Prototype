using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Player Settings", menuName ="Platformer/Player")]
public class PlayerSettings : ScriptableObject {
	[SerializeField] private float defaultGravityScale = 1.3f;

	[Header("Ground Variables")]
	[SerializeField] private float maxGroundSpeed = 10f;
	[SerializeField] private float groundCheckRadius = 0.05f;
	[SerializeField] private Vector2 groundedJumpForce = new Vector2(0f, 10f);
	[SerializeField] private LayerMask whatIsGround;

	[Header ("Air Variables")]
	[SerializeField] private float maxAirSpeed = 10f;
	[SerializeField] private float doubleJumpDelaySeconds = 0.1f;
	[SerializeField] private float glidingGravity = 0.8f;
	[SerializeField] private Vector2 doubleJumpForce = new Vector2(0f, 10f);

	[Header("Wall Variables")]
	[SerializeField] private float wallClimbSpeed = 3f;
	[SerializeField] private float wallCheckRadius = 0.5f;
	[SerializeField] private float wallGripThreshold = 0.1f;
	[SerializeField] private Vector2 wallJumpForce = new Vector2(0f, 10f);
	[SerializeField] private LayerMask whatIsWall;

	[Header ("Dash Variables")]
	[SerializeField] private Vector2 dashVector = new Vector2(600, 0f);
	[SerializeField] private float lastDashTime = 0f;
	[SerializeField] private float dashCoolDownSeconds = 0.5f;

	private Transform groundCheckPoint;
	private Transform wallCheckPoint;

	public float DefaultGravityScale { get { return defaultGravityScale; } }

	public Transform GroundCheckPoint { get { return groundCheckPoint; } set { groundCheckPoint = value; } }
	public float MaxGroundSpeed { get { return maxGroundSpeed; } }
	public float GroundCheckRadius { get { return groundCheckRadius; } }
	public Vector2 GroundJumpForce { get { return groundedJumpForce; } }
	public LayerMask WhatIsGround { get { return whatIsGround; } }

	public float MaxAirSpeed { get { return maxAirSpeed; } }
	public float DoubleJumpDelaySeconds { get { return doubleJumpDelaySeconds; } }
	public float GlidingGravity { get { return glidingGravity; } }
	public Vector2 DoubleJumpForce { get { return doubleJumpForce; } }

	public Transform WallCheckPoint { get { return wallCheckPoint; } set { wallCheckPoint = value; } }
	public float WallClimbSpeed { get { return wallClimbSpeed; } }
	public float WallCheckRadius { get { return wallCheckRadius; } }
	public float WallGripThreshold { get { return wallGripThreshold; } }
	public Vector2 WallJumpForce { get { return wallJumpForce; } }
	public LayerMask WhatIsWall { get { return whatIsWall; } }

	public Vector2 DashVector { get { return dashVector; } }
	public float LastDashTime { get { return lastDashTime; } set { lastDashTime = value; } }
	public float DashCooldownSeconds { get { return dashCoolDownSeconds; } }

	private void OnEnable()
	{
		lastDashTime = 0f;
	}
}
