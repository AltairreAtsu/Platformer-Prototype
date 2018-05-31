using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour {
	[SerializeField] private float collisionDamage = 1;
	[SerializeField] private int EnemyLayer;

	private Collider2D playerCollider;
	private Collider2D lastEnemyTriggerEntered;
	private MusicPlayer musicPlayer;
	private bool collidingWithLast = false;

	public delegate void CollisionEvent(float damage, Transform damageDealer);
	public event CollisionEvent enemyCollisionEvent;

	private void Start()
	{
		// TODO inject this dependency
		musicPlayer = GameObject.FindGameObjectWithTag("Music Player").GetComponent<MusicPlayer>();
		playerCollider = GetComponent<Collider2D>();
	}

	private void Update()
	{
		if (lastEnemyTriggerEntered == null && collidingWithLast)
		{
			collidingWithLast = false;
			musicPlayer.TransitionBackToDefault();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ( collision.gameObject.layer == EnemyLayer)
		{
			if (enemyCollisionEvent != null){ enemyCollisionEvent(collisionDamage, collision.gameObject.transform); }
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if( collision.gameObject.layer == EnemyLayer)
		{
			lastEnemyTriggerEntered = collision;
			musicPlayer.TransitionToAggresive();
			collidingWithLast = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if( other == lastEnemyTriggerEntered)
		{
			musicPlayer.TransitionBackToDefault();
			collidingWithLast = false;
		}
	}
}
