using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent (typeof(Animator))]
public class FadeCanvas : MonoBehaviour {

	public StringEvent finishedFading;
	public UnityEvent quitFadeEvent;

	private string levelName;
	private bool quitOnFade = false;

	public void cacheLevelName(string levelName)
	{
		this.levelName = levelName;
	}
	public void QuitOnFadeOut()
	{
		quitOnFade = true;
	}

	public void AnimatorFadeComplete()
	{
		if (quitOnFade) { quitFadeEvent.Invoke(); return; }
		finishedFading.Invoke(levelName);
	}
}

[Serializable]
public class StringEvent : UnityEvent<string> { }
