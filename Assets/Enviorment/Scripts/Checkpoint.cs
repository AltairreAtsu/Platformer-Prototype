using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.GetComponentInParent<PlayerController>();
		
		if (player)
		{
			player.SetCheckPoint(this);
		}
	}
}
