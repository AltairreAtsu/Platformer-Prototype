using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var colliderParent = collision.gameObject.transform.parent;
		if (colliderParent && colliderParent.tag == "Player")
		{
			colliderParent.GetComponent<PlayerMovement>().SetCheckPoint(this);
		}
	}
}
