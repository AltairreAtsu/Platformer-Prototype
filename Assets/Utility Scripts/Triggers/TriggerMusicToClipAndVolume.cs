using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusicToClipAndVolume : MonoBehaviour
{
	private MusicPlayer musicPlayer;

	[SerializeField] private string regionName;
	[SerializeField] private float durration;
	[SerializeField] private float volume;

	private void Start()
	{
		musicPlayer = FindObjectOfType<MusicPlayer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.GetComponentInParent<PlayerController>();

		if (player)
		{
			musicPlayer.DoRegionTransition(regionName, durration, volume);
		}
	}
}
