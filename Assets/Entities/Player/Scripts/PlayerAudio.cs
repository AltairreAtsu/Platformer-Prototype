using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAudio : MonoBehaviour {
	[SerializeField] private int surfaceAudioPlayerPriority = 128;
	[SerializeField] private int jumpAudioPlayerPriority = 127;
	[SerializeField] private int attackAudioPlayerPriority = 126;
	[Space]
	[SerializeField] private AudioPlayer attackSoundPlayer;
	[SerializeField] private AudioPlayer jumpSoundPlayer;
	[SerializeField] private AudioPlayer dashSoundPlayer;
	[SerializeField] private AudioPlayer glideSoundPlayer;
	[Space]
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

		playerController.attackEvent += OnAttack;
		playerLocomotion.jumpEvent += OnJump;
		playerLocomotion.dashEvent += OnDash;
		playerLocomotion.glideEvent += OnGlide;
		playerController.LandEvent += OnLand;
	}

	public void PlayWalkSound()
	{
		var tile = playerController.GetTileSurfaceBeneath();
		if(tile == null) { return;  }

		var surface = surfaceManager.GetSurface(tile.name);
		if(surface == null) { return; }
		surface.PlayWalkSound(audioSource);
	}

	public void PlayClimbingSound()
	{
		var tile = playerController.GetTileSurfaceClimbing();
		if (tile == null) { return; }

		var surface = surfaceManager.GetSurface(tile.name);
		if (surface == null) { return; }
		surface.PlayWalkSound(audioSource);
	}

	private void OnAttack()
	{
		attackSoundPlayer.Play(audioSource);
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

	private void OnLand(PhysicsMaterial2D material)
	{
		if (material == null)
		{
			var tile = playerController.GetTileSurfaceBeneath();

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
	}
}
