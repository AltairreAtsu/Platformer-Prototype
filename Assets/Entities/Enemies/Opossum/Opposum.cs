using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opposum : MonoBehaviour {
	[SerializeField] private EntitySettings entitySettings;
	[Space]
	[SerializeField] private Vector2 startPoint;
	[SerializeField] private Vector2 endPoint;
	[SerializeField] private float pointArrivalDistance = .3f;
	[Space]
	[SerializeField] private Transform groundCheckPoint;

	private Rigidbody2D rigidbody2d;
	private Vector3 originalPosition;
	private Vector2 currentTarget;
	private bool facingRight = false;

	void Start () {
		rigidbody2d = GetComponent<Rigidbody2D>();
		originalPosition = transform.position;
		currentTarget = startPoint + (Vector2)originalPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsTouchingGround()) { return;  }
		float distanceToTarget = (transform.position - (Vector3)currentTarget).magnitude;
		if (distanceToTarget < pointArrivalDistance)
		{
			currentTarget = (currentTarget == startPoint + (Vector2)originalPosition) ? endPoint + (Vector2)originalPosition : startPoint + (Vector2)originalPosition;
		}

		var steering = SteeringBehaviors.Arrival(rigidbody2d.velocity, currentTarget, transform.position, entitySettings.MaxSpeed, entitySettings.SlowingRadius);

		steering = Vector2.ClampMagnitude(steering, entitySettings.MaxForce);
		steering = steering / rigidbody2d.mass;
		rigidbody2d.AddForce(steering);
		rigidbody2d.velocity = Vector2.ClampMagnitude(rigidbody2d.velocity, entitySettings.MaxVelocity);
		CheckCharacterFlip();
	}

	public bool IsTouchingGround()
	{
		Collider2D[] groundColliders =
			Physics2D.OverlapCircleAll(groundCheckPoint.position, entitySettings.GroundCheckRadius, entitySettings.WhatISGround);

		if (groundColliders.Length > 0)
		{
			return true;
		}
		return false;
	}

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
