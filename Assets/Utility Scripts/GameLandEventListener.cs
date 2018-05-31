using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLandEventListener : MonoBehaviour
{
	[SerializeField] private GameLandEvent Event;
	[SerializeField] private UnityPhysicsEvent response;

	private void OnEnable()
	{
		Event.RegisterListener(this);
	}

	private void OnDisable()
	{
		Event.UnRegisterListener(this);
	}

	public void OnEventRaise(PhysicsMaterial2D material)
	{
		response.Invoke(material);
	}
}

[System.Serializable]
public class UnityPhysicsEvent : UnityEvent<PhysicsMaterial2D> {}
