using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	[SerializeField] private AudioClip jumpSound = null;
	[SerializeField] private AudioClip dashSound = null;

	private AudioSource audioSource;
	private PlayerMovement playerMovement;

	private void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		playerMovement = GetComponent<PlayerMovement>();

		playerMovement.jumpEvent += OnJump;
		playerMovement.dashEvent += OnDash;
	}

	private void OnJump ()
	{
		audioSource.clip = jumpSound;
		audioSource.Play();
	}

	private void OnDash()
	{
		audioSource.clip = dashSound;
		audioSource.Play();
	}

	private void OnDisable()
	{
		playerMovement.jumpEvent -= OnJump;
	}
}
