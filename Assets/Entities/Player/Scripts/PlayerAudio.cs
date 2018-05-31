using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAudio : MonoBehaviour {
	#region Serialized Fields
	[Header ("Audio Players")]
	[SerializeField] private AudioPlayer attackSoundPlayer;
	[SerializeField] private AudioPlayer jumpSoundPlayer;
	[SerializeField] private AudioPlayer dashSoundPlayer;
	[SerializeField] private AudioPlayer glideSoundPlayer;
	[Header ("Scriptable Objects")]
	[SerializeField] private PlayerSettings playerSettings;
	[SerializeField] private SurfaceManager surfaceManager;
	[Header("Dependencies")]
	[SerializeField] PlayerController playerController;
	#endregion

	private AudioSource audioSource;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayWalkSound()
	{
		var tile = playerController.GetTileSurfaceBeneath();
		if(tile == null) { return;  }

		var surface = surfaceManager.GetSurface(tile);
		if(surface == null) { return; }
		surface.PlayWalkSound(audioSource);
	}

	public void PlayClimbingSound()
	{
		var tile = playerController.GetTileSurfaceClimbing();
		if (tile == null) { return; }

		var surface = surfaceManager.GetSurface(tile);
		if (surface == null) { return; }
		surface.PlayWalkSound(audioSource);
	}

	public void OnAttack()
	{
		attackSoundPlayer.Play(audioSource);
	}

	public void OnJump ()
	{
		jumpSoundPlayer.Play(audioSource);
	}

	public void OnDash()
	{

		dashSoundPlayer.Play(audioSource);
	}

	public void OnGlide()
	{
		glideSoundPlayer.Play(audioSource);
	}

	public void OnLand(PhysicsMaterial2D material)
	{
		if (material == null)
		{
			var tile = playerController.GetTileSurfaceBeneath();

			if (tile == null) { return; }

			var surface = surfaceManager.GetSurface(tile);
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
}
