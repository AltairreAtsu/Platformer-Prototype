using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New String Event", menuName ="Infrastructure/String Game Event")]
[System.Serializable]
public class GameEventString : ScriptableObject
{
	public List<StringEventListener> eventListeners = new List<StringEventListener>();

	public void Raise(string stringInput)
	{
		for (int i = eventListeners.Count - 1; i >= 0; i--)
		{
			eventListeners[i].OnEventRaise(stringInput);
		}
	}

	public void RegisterListener(StringEventListener eventListener)
	{
		eventListeners.Add(eventListener);
	}
	public void UnRegisterListener(StringEventListener eventListener)
	{
		eventListeners.Remove(eventListener);
	}
}