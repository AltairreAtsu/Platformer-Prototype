using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Entity Settings", menuName ="Platformer/Entity Settings")]
public class EntitySettings : ScriptableObject {
	[SerializeField] private float maxSpeed = 10f;
	[SerializeField] private float maxVelocity = 10f;
	[SerializeField] private float maxForce = 10f;
	[SerializeField] private float slowingRadius = 2f;
	[SerializeField] private float groundCheckRadius = .05f;

	[SerializeField] private LayerMask whatIsGround;

	public float MaxSpeed { get { return maxSpeed; } }
	public float MaxVelocity { get { return maxVelocity; } }
	public float MaxForce { get { return maxForce; } }
	public float SlowingRadius { get { return slowingRadius; } }
	public float GroundCheckRadius { get { return groundCheckRadius; } }

	public LayerMask WhatISGround { get { return whatIsGround; } }
}
