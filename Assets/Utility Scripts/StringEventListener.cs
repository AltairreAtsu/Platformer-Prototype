using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringEventListener : MonoBehaviour
{
	[SerializeField] private GameEventString Event;
	[SerializeField] private UnityStringEvent response;

	private void OnEnable()
	{
		Event.RegisterListener(this);
	}

	private void OnDisable()
	{
		Event.UnRegisterListener(this);
	}

	public void OnEventRaise(string stringInput)
	{
		response.Invoke(stringInput);
	}
}

[System.Serializable]
public class UnityStringEvent : UnityEvent<string> {}
