using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAudio : MonoBehaviour {
	[SerializeField] private AudioPlayer jumpSoundPlayer;
	[SerializeField] private AudioPlayer dashSoundPlayer;
	[SerializeField] private AudioPlayer glideSoundPlayer;
	[SerializeField] private AudioPlayer dieSoundPlayer;

	[SerializeField] private PlayerSettings playerSettings;
	[SerializeField] private SurfacManager surfaceManager;

	private AudioSource audioSource;
	private PlayerController playerController;
	private PlayerLocomotion playerLocomotion;

	private void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		playerController = GetComponent<PlayerController>();
		playerLocomotion = GetComponent<PlayerLocomotion>();

		playerLocomotion.jumpEvent += OnJump;
		playerLocomotion.dashEvent += OnDash;
		playerLocomotion.glideEvent += OnGlide;
		playerController.LandEvent += OnLand;
		playerController.dieEvent += OnDie;
	}

	public void PlayWalkSound()
	{
		var tile = playerController.GetSurfaceBeneath();
		if(tile == null) { return;  }

		var surface = surfaceManager.GetSurface(tile.name);
		if(surface == null) { return; }
		surface.PlayWalkSound(audioSource);
	}

	public void PlayClimbingSound()
	{
		var tile = playerController.GetSurfaceClimbing();
		if (tile == null) { return; }

		var surface = surfaceManager.GetSurface(tile.name);
		if (surface == null) { return; }
		surface.PlayWalkSound(audioSource);
	}

	private void OnJump ()
	{
		jumpSoundPlayer.Play(audioSource);
	}

	private void OnDash()
	{
		dashSoundPlayer.Play(audioSource);
	}

	private void OnGlide()
	{
		glideSoundPlayer.Play(audioSource);
	}

	private void OnDie()
	{
		dieSoundPlayer.Play(audioSource);
	}

	private void OnLand(PhysicsMaterial2D material)
	{
		if (material == null)
		{
			var tile = playerController.GetSurfaceBeneath();

			if (tile == null) { return; }

			var surface = surfaceManager.GetSurface(tile.name);
			if (surface == null) { return; }
			surface.PlayLandSound(audioSource);
		}
		else
		{
			var surface = surfaceManager.GetSurface(material);
			if (surface == null) { return; }
			surface.PlayLandSound(audioSource);
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
