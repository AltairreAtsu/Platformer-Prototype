using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Infrastructure/Game Land Event")]
public class GameLandEvent : ScriptableObject {
	public List<GameLandEventListener> eventListeners = new List<GameLandEventListener>();

	public void Raise(PhysicsMaterial2D material)
	{
		for (int i = eventListeners.Count - 1; i >= 0; i--)
		{
			eventListeners[i].OnEventRaise(material);
		}
	}

	public void RegisterListener(GameLandEventListener eventListener)
	{
		eventListeners.Add(eventListener);
	}
	public void UnRegisterListener(GameLandEventListener eventListener)
	{
		eventListeners.Remove(eventListener);
	}
}
