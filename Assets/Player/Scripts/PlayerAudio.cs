using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAudio : MonoBehaviour {
	[SerializeField] private int surfaceAudioPlayerPriority = 128;
	[SerializeField] private int jumpAudioPlayerPriority = 127;
	[Space]
	[SerializeField] private AudioPlayer jumpSoundPlayer;
	[SerializeField] private AudioPlayer dashSoundPlayer;
	[SerializeField] private AudioPlayer glideSoundPlayer;
	[SerializeField] private AudioPlayer dieSoundPlayer;
	[Space]
	[SerializeField] private PlayerSettings playerSettings;
	[SerializeField] private SurfacManager surfaceManager;

	private AudioSource surfaceAudioSource;
	private AudioSource jumpAudioSource;

	private PlayerController playerController;
	private PlayerLocomotion playerLocomotion;

	private void Start ()
	{
		GetAudioSources();

		playerController = GetComponent<PlayerController>();
		playerLocomotion = GetComponent<PlayerLocomotion>();

		playerLocomotion.jumpEvent += OnJump;
		playerLocomotion.dashEvent += OnDash;
		playerLocomotion.glideEvent += OnGlide;
		playerController.LandEvent += OnLand;
		playerController.dieEvent += OnDie;
	}

	private void GetAudioSources()
	{
		var audioSources = GetComponents<AudioSource>();
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioSources[i].priority == surfaceAudioPlayerPriority) { surfaceAudioSource = audioSources[i]; }
			if (audioSources[i].priority == jumpAudioPlayerPriority) { jumpAudioSource = audioSources[i]; }
		}
	}

	public void PlayWalkSound()
	{
		var tile = playerController.GetSurfaceBeneath();
		if(tile == null) { return;  }

		var surface = surfaceManager.GetSurface(tile.name);
		if(surface == null) { return; }
		surface.PlayWalkSound(surfaceAudioSource);
	}

	public void PlayClimbingSound()
	{
		var tile = playerController.GetSurfaceClimbing();
		if (tile == null) { return; }

		var surface = surfaceManager.GetSurface(tile.name);
		if (surface == null) { return; }
		surface.PlayWalkSound(surfaceAudioSource);
	}

	private void OnJump ()
	{
		jumpSoundPlayer.Play(jumpAudioSource);
	}

	private void OnDash()
	{
		dashSoundPlayer.Play(surfaceAudioSource);
	}

	private void OnGlide()
	{
		glideSoundPlayer.Play(surfaceAudioSource);
	}

	private void OnDie()
	{
		dieSoundPlayer.Play(surfaceAudioSource);
	}

	private void OnLand(PhysicsMaterial2D material)
	{
		if (material == null)
		{
			var tile = playerController.GetSurfaceBeneath();

			if (tile == null) { return; }

			var surface = surfaceManager.GetSurface(tile.name);
			if (surface == null) { return; }
			surface.PlayLandSound(surfaceAudioSource);
		}
		else
		{
			var surface = surfaceManager.GetSurface(material);
			if (surface == null) { return; }
			surface.PlayLandSound(surfaceAudioSource);
		}
	}

	private void OnDisable()
	{
		playerLocomotion.jumpEvent -= OnJump;
		playerLocomotion.dashEvent -= OnDash;
		playerController.LandEvent -= OnLand;
		playerController.dieEvent -= OnDie;
	}
}
