using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Eagle : MonoBehaviour {
	[SerializeField] private Vector2 startPoint;
	[SerializeField] private Vector2 endPoint;
	[SerializeField] private float pointArrivalDistance = .3f;
	[Space]
	[SerializeField] private float maxSpeed = 10f;
	[SerializeField] private float maxVelocity = 10f;
	[SerializeField] private float maxForce = 10f;
	[Space]
	[SerializeField] private float slowdingRadius = 2f;
	[Space]
	[SerializeField] private float maxCollsionAvoidanceLookAhead = 2f;
	[Space]
	[SerializeField] private bool seekPlayer = false;

	private Transform playerTransform;

	private Animator animator;
	private Rigidbody2D rigidbody2d;
	private Tilemap foreground;
	private Vector2 currentTarget;

	private bool facingRight = false;

	private void Start ()
	{
		playerTransform = GameObject.FindWithTag("Player").transform;
		foreground = GameObject.FindWithTag("Foreground").GetComponent<Tilemap>();

		animator = GetComponent<Animator>();
		rigidbody2d = GetComponent<Rigidbody2D>();
		currentTarget = startPoint;
	}
	
	private void Update ()
	{
		if (!seekPlayer)
		{
			float distanceToTarget = (transform.position - (Vector3)currentTarget).magnitude;
			if (distanceToTarget < pointArrivalDistance)
			{
				currentTarget = (currentTarget == startPoint) ? endPoint : startPoint;
			}
		}
		else
		{
			currentTarget = playerTransform.position;
		}


		var steering = SteeringBehaviors.Arrival(rigidbody2d.velocity, currentTarget, transform.position, maxSpeed, slowdingRadius);

		steering = Vector2.ClampMagnitude(steering, maxForce);
		steering = steering / rigidbody2d.mass;
		rigidbody2d.velocity = Vector2.ClampMagnitude(rigidbody2d.velocity + steering, maxVelocity);
		CheckCharacterFlip();
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
}
