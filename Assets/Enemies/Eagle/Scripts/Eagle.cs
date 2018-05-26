using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Eagle : MonoBehaviour {
	[SerializeField] private Vector2 startPoint;
	[SerializeField] private Vector2 endPoint;
	[SerializeField] private float maxSpeed = 10f;
	[SerializeField] private float maxVelocity = 10f;
	[SerializeField] private float maxForce = 10f;
	[SerializeField] private float slowdingRadius = 2f;

	private Transform playerTransform;

	private Animator animator;
	private Rigidbody2D rigidbody2d;
	private Tilemap foreground;

	private bool facingRight = false;
	private bool movingRight = false;

	void Start ()
	{
		playerTransform = GameObject.FindWithTag("Player").transform;
		foreground = GameObject.FindWithTag("Foreground").GetComponent<Tilemap>();

		animator = GetComponent<Animator>();
		rigidbody2d = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		// Move the Eagle between the start and End Points
		var steering = Arrival(rigidbody2d.velocity, playerTransform.position, transform.position, maxSpeed, slowdingRadius);
		steering = Vector2.ClampMagnitude(steering, maxForce);
		steering = steering / rigidbody2d.mass;
		rigidbody2d.velocity = Vector2.ClampMagnitude(rigidbody2d.velocity + steering, maxVelocity);
		CheckCharacterFlip();
	}

	public Vector2 Seek (Vector2 velocity, Vector2 targetPosition, Vector2 selfPosition, float maxVelocity)
	{
		var desiredVelocity = targetPosition - selfPosition;
		desiredVelocity = desiredVelocity.normalized * maxVelocity;
		return desiredVelocity - velocity;
	}

	public Vector2 Flee (Vector2 velocity, Vector2 targetPosition, Vector2 selfPosition, float maxVelocity)
	{
		var desiredVelocity = selfPosition - targetPosition;
		desiredVelocity = desiredVelocity.normalized * maxVelocity;
		return desiredVelocity - velocity;
	}

	public Vector2 Arrival(Vector2 velocity, Vector2 targetPosition, Vector2 selfPosition, float maxVelocity, float slowingRadius)
	{
		var desiredVelocity = targetPosition - selfPosition;
		var distance = desiredVelocity.magnitude;

		if(distance < slowingRadius)
		{
			desiredVelocity = desiredVelocity.normalized * maxVelocity * (distance / slowingRadius);
			return desiredVelocity - velocity;
		}
		else
		{
			desiredVelocity = desiredVelocity.normalized * maxVelocity;
			return desiredVelocity - velocity;
		}
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
