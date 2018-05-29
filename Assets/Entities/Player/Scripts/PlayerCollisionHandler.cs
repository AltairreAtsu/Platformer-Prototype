using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour {
	[SerializeField] private float collisionDamage = 1;
	[SerializeField] private int EnemyLayer;

	public delegate void CollisionEvent(float damage, Transform damageDealer);
	public event CollisionEvent enemyCollisionEvent;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ( collision.gameObject.layer == EnemyLayer)
		{
			if (enemyCollisionEvent != null) { enemyCollisionEvent(collisionDamage, collision.gameObject.transform); }
		}
	}
}
