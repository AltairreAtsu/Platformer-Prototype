using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour {
	[SerializeField] StringAudioClipDictionary UISoundLibrary;

	[SerializeField] private AudioClip hoverSound;

	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySound(string buttonName)
	{
		// Ensure the switch statement won't start before the audioSource is found
		// TODO: Find some way to stop the Menu from calling the Unity Event when it's value is changed in code.
		if (audioSource == null) { return;  }

		AudioClip clip = null;
		UISoundLibrary.TryGetValue(buttonName, out clip);

		if(clip == null)
		{
			Debug.LogWarning("Could not find Audio Clip at provided key: " + buttonName);
		}

		audioSource.PlayOneShot(clip);
	}

	public void PlayHoverSound()
	{
		audioSource.Stop();
		audioSource.clip = hoverSound;
		audioSource.Play();
	}
}
