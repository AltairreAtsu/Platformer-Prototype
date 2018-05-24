using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	[SerializeField] private AudioPlayer jumpSoundPlayer;
	[SerializeField] private AudioPlayer dashSoundPlayer;
	[SerializeField] private AudioPlayer dieSoundPlayer;
	[SerializeField] private AudioPlayer landSoundGrassPlayer;

	private AudioSource audioSource;
	private PlayerMovement playerMovement;

	private void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		playerMovement = GetComponent<PlayerMovement>();

		InjectAudioSource(jumpSoundPlayer, dashSoundPlayer, dieSoundPlayer, landSoundGrassPlayer);

		playerMovement.jumpEvent += OnJump;
		playerMovement.dashEvent += OnDash;
		playerMovement.LandEvent += OnLand;
		playerMovement.dieEvent += OnDie;
	}

	private void InjectAudioSource(params AudioPlayer[] audioPlayers)
	{
		for (int i = 0; i < audioPlayers.Length; i++)
		{
			audioPlayers[i].audioSource = audioSource;
		}
	}

	private void OnJump ()
	{
		jumpSoundPlayer.Play();
	}

	private void OnDash()
	{
		dashSoundPlayer.Play();
	}

	private void OnDie()
	{
		dieSoundPlayer.Play();
	}

	private void OnLand(PhysicsMaterial2D material)
	{
		if (material == null)
		{
			landSoundGrassPlayer.Play();
			return;
		}

		if (material.name == "Bouncy")
		{
			jumpSoundPlayer.Play();
			return;
		}
	}

	private void OnDisable()
	{
		playerMovement.jumpEvent -= OnJump;
	}
}
