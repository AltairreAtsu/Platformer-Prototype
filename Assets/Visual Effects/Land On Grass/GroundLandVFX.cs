﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLandVFX : MonoBehaviour {

	private Animator animator;
	private PlayerMovement playerMovement;

	private void Start()
	{
		animator = GetComponent<Animator>();
		playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		playerMovement.LandEvent += OnLand;
	}

	private void OnLand(PhysicsMaterial2D material)
	{
		animator.SetBool("Land", true);
		transform.position = playerMovement.transform.position;
	}

	public void OnLanded()
	{
		animator.SetBool("Land", false);
	}

}
