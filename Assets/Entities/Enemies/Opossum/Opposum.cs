using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Opposum : MonoBehaviour {
	[SerializeField] private EntitySettings entitySettings;
	[SerializeField] private float WorldBottomThreshold;
	[Space]
	[SerializeField] private Vector2 startPoint;
	[SerializeField] private Vector2 endPoint;
	[SerializeField] private float pointArrivalDistance = .3f;
	[Space]
	[SerializeField] private Transform groundCheckPoint;

	private IDamagable healthScript;
	private Rigidbody2D rigidbody2d;

	private Transform playerTransform;
	private bool chasingPlayer = false;

	private Vector3 originalPosition;
	private Vector2 currentTarget;

	private bool facingRight = false;

	void Start ()
	{
		SubscribeToDamageEvent();

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		rigidbody2d = GetComponent<Rigidbody2D>();
		originalPosition = transform.position;
		currentTarget = startPoint + (Vector2)originalPosition;
	}

	private void SubscribeToDamageEvent()
	{
		healthScript = GetComponent<IDamagable>();
		healthScript.DamageEvent += OnDamage;
	}

	void Update ()
	{
		// TODO, Enemy Pooling
		if (FellOutOfWorld()) { healthScript.Die(); }
		if (!IsTouchingGround(groundCheckPoint.position)) { return; }
		
		

		Vector2 steering;

		if (!chasingPlayer)
		{
			steering = Patrol();
		}
		else
		{
			steering = PursuePlayer();
		}
		

		steering = UpdateVelocity(steering);
	}

	private bool FellOutOfWorld()
	{
		return (transform.position.y < WorldBottomThreshold);
	}

	private Vector2 PursuePlayer()
	{
		// This checks the player is above ground the opossum can theoretically reach
		var checkPlayerGroundedPoint = playerTransform.position;
		checkPlayerGroundedPoint.y = groundCheckPoint.position.y;

		if ( IsTouchingGround(checkPlayerGroundedPoint))
		{
			return SteeringBehaviors.Arrival(rigidbody2d.velocity, checkPlayerGroundedPoint, transform.position, entitySettings.MaxSpeed, entitySettings.SlowingRadius);
		}
		else
		{
			Debug.Log("Running!");
			chasingPlayer = false;
			return Patrol();
		}

	}

	private Vector2 Patrol()
	{
		float distanceToTarget = (transform.position - (Vector3)currentTarget).magnitude;
		if (distanceToTarget < pointArrivalDistance)
		{
			currentTarget = (currentTarget == startPoint + (Vector2)originalPosition) ? endPoint + (Vector2)originalPosition : startPoint + (Vector2)originalPosition;
		}

		return SteeringBehaviors.Arrival(rigidbody2d.velocity, currentTarget, transform.position, entitySettings.MaxSpeed, entitySettings.SlowingRadius);
	}

	private Vector2 UpdateVelocity(Vector2 steering)
	{
		steering = Vector2.ClampMagnitude(steering, entitySettings.MaxForce);
		steering = steering / rigidbody2d.mass;
		rigidbody2d.AddForce(steering);
		rigidbody2d.velocity = Vector2.ClampMagnitude(rigidbody2d.velocity, entitySettings.MaxVelocity);
		CheckCharacterFlip();
		return steering;
	}

	public bool IsTouchingGround(Vector2 groundCheckPosition)
	{
		Collider2D[] groundColliders =
			Physics2D.OverlapCircleAll(groundCheckPosition, entitySettings.GroundCheckRadius, entitySettings.WhatISGround);

		if (groundColliders.Length > 0)
		{
			return true;
		}
		return false;
	}

	private void OnDamage(System.Object Object, EventArgs e)
	{
		chasingPlayer = true;
	}

	// TODO: Migrate this to a commoon Entity Class
	public void CheckCharacterFlip()
	{
		if (rigidbody2d.velocity.x < 0 && facingRight)
		{
			facingRight = false;
			Flip();
		}
		else if (rigidbody2d.velocity.x > 0 && !facingRight)
		{
			facingRight = true;
			Flip();
		}
	}
	public void Flip()
	{
		transform.localScale = new Vector3(transform.localScale.x * -1,
											transform.localScale.y,
											transform.localScale.z
			);
	}

	private void OnDrawGizmos()
	{
		if (startPoint == endPoint) { return; };
		if(originalPosition == Vector3.zero && !Application.isPlaying) { originalPosition = transform.position; }
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine((Vector3)startPoint + originalPosition + transform.up * .2f, (Vector3)endPoint + originalPosition + transform.up * .2f);
		Gizmos.DrawSphere((Vector3)startPoint + originalPosition + transform.up * .2f, 0.2f);
		Gizmos.DrawSphere((Vector3)endPoint + originalPosition + transform.up * .2f, 0.2f);
	}
}
