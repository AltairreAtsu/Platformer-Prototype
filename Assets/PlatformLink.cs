using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLink : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.GetComponentInParent<PlayerController>();
		if (player != null)
		{
			player.SetTransformParent(transform);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		var player = collision.GetComponentInParent<PlayerController>();
		if (player != null)
		{
			player.UnSetTransformParent();
		}
	}
}
