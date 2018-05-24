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
	private PlayerMovement playerMovement;

	private void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		playerMovement = GetComponent<PlayerMovement>();

		playerMovement.jumpEvent += OnJump;
		playerMovement.dashEvent += OnDash;
		playerMovement.glideEvent += OnGlide;
		playerMovement.LandEvent += OnLand;
		playerMovement.dieEvent += OnDie;
	}

	public void PlayWalkSound()
	{
		var tile = playerMovement.GetSurfaceBeneath();
		if(tile == null) { return;  }

		var surface = surfaceManager.GetSurface(tile.name);
		if(surface == null) { return; }
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
			var tile = playerMovement.GetSurfaceBeneath();

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
		playerMovement.jumpEvent -= OnJump;
		playerMovement.dashEvent -= OnDash;
		playerMovement.LandEvent -= OnLand;
		playerMovement.dieEvent -= OnDie;
	}
}
