using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderEvent : MonoBehaviour
{
	[SerializeField] private GameEventString eventToRaiseOnEnter;
	[SerializeField] private GameEvent eventToRaiseOnExit;
	[SerializeField] private string cueName;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.GetComponentInParent<PlayerController>();

		if (player)
		{
			eventToRaiseOnEnter.Raise(cueName);
		}
	}

	private void OnTriggerExit2D (Collider2D collider)
	{
		var player = collider.GetComponentInParent<PlayerController>();

		if (player)
		{
			eventToRaiseOnExit.Raise();
		}
	}
}
