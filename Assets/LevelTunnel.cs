using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTunnel : MonoBehaviour {

	[SerializeField] private LevelTunnel exitTunnel;
	[SerializeField] private bool enterFromRight;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var parentObject = collision.gameObject.transform.parent;
		if (!parentObject || parentObject.tag != "Player") { return; }

		var playerMovement = parentObject.GetComponent<PlayerMovement>();
		if (playerMovement.FacingRight == enterFromRight)
		{
			playerMovement.Teleport(exitTunnel.transform.position);
		}
	}

}
