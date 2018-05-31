using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Game Event", menuName ="Infrastructure/Game Event")]
public class GameEvent : ScriptableObject
{
	public List<GameEventListener> eventListeners = new List<GameEventListener>();

	public virtual void Raise()
	{
		for(int i = eventListeners.Count -1; i >= 0; i--)
		{
			eventListeners[i].OnEventRaise();
		}
	}
	
	public void RegisterListener( GameEventListener eventListener)
	{
		eventListeners.Add(eventListener);
	}
	public void UnRegisterListener(GameEventListener eventListener)
	{
		eventListeners.Remove(eventListener);
	}
}
