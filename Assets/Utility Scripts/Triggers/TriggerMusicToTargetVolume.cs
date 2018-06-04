using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusicToTargetVolume : MonoBehaviour
{
	private MusicPlayer musicPlayer;

	[SerializeField] private float volumeToTransitionTo;
	[SerializeField] private float durrationOfTransition;

	private void Start()
	{
		musicPlayer = FindObjectOfType<MusicPlayer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.GetComponentInParent<PlayerController>();

		if (player)
		{
			musicPlayer.DoTransitionToVolume(volumeToTransitionTo, durrationOfTransition);
		}
	}
}
