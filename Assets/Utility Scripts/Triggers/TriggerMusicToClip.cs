using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusicToClip : MonoBehaviour
{
	private MusicPlayer musicPlayer;

	[SerializeField] private AudioClip clip;
	[SerializeField] private float durration;

	private void Start()
	{
		musicPlayer = FindObjectOfType<MusicPlayer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.GetComponentInParent<PlayerController>();

		if (player)
		{
			musicPlayer.StartTransition(clip, durration);
		}
	}
}
