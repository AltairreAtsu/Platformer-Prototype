using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour {
	[SerializeField] private AudioClip defaultClickSound;
	[SerializeField] private AudioClip playClickSound;
	[SerializeField] private AudioClip quitClickSound;

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

		switch (buttonName)
		{
			case "Play":
				audioSource.Stop();
				audioSource.clip = playClickSound;
				audioSource.Play();
				break;
			case "Quit":
				audioSource.Stop();
				audioSource.clip = quitClickSound;
				audioSource.Play();
				break;
			default:
				audioSource.Stop();
				audioSource.clip = defaultClickSound;
				audioSource.Play();
				break;
		}
	}

	public void PlayHoverSound()
	{
		audioSource.Stop();
		audioSource.clip = hoverSound;
		audioSource.Play();
	}
}
