using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAudio : MonoBehaviour {
	[SerializeField] private AudioPlayer jumpSoundPlayer;
	[SerializeField] private AudioPlayer dashSoundPlayer;
	[SerializeField] private AudioPlayer glideSoundPlayer;
	[SerializeField] private AudioPlayer dieSoundPlayer;
	[SerializeField] private AudioPlayer landSoundGrassPlayer;
	[SerializeField] private AudioPlayer landSoundWoodPlayer;
	[SerializeField] private AudioPlayer walkSoundGrassPlayer;
	[SerializeField] private AudioPlayer walkSoundWoodPlayer;
	[SerializeField] private PlayerSettings playerSettings;

	[SerializeField] private RuleTile GrassTile;
	[SerializeField] private RuleTile WoodTile;

	private AudioSource audioSource;
	private PlayerMovement playerMovement;

	private void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		playerMovement = GetComponent<PlayerMovement>();

		InjectAudioSource(jumpSoundPlayer, dashSoundPlayer, glideSoundPlayer, dieSoundPlayer, landSoundGrassPlayer, landSoundWoodPlayer, walkSoundGrassPlayer, walkSoundWoodPlayer);

		playerMovement.jumpEvent += OnJump;
		playerMovement.dashEvent += OnDash;
		playerMovement.glideEvent += OnGlide;
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

	public void PlayWalkSound()
	{
		var tile = playerMovement.GetSurfaceBeneath();
		if(tile == null) { return;  }
		switch (tile.name)
		{
			case "Wood":
				walkSoundWoodPlayer.Play();
				break;
			case "Grass and Stone":
			default:
				walkSoundGrassPlayer.Play();
				break;
		}
		
		// TODO Unbind this form gras and add logic to switch step sound based on material
		//walkSoundGrassPlayer.Play();
	}

	private void OnJump ()
	{
		jumpSoundPlayer.Play();
	}

	private void OnDash()
	{
		dashSoundPlayer.Play();
	}

	private void OnGlide()
	{
		glideSoundPlayer.Play();
	}

	private void OnDie()
	{
		dieSoundPlayer.Play();
	}

	private void OnLand(PhysicsMaterial2D material)
	{
		if (material == null)
		{
			var tile = playerMovement.GetSurfaceBeneath();

			if (tile == null) { return; }

			switch (tile.name)
			{
				case "Wood":
					landSoundWoodPlayer.Play();
					break;
				case "Grass and Stone":
				default:
					landSoundGrassPlayer.Play();
					break;
			}
		}
		else if (material.name == "Bouncy")
		{
			jumpSoundPlayer.Play();
			return;
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
