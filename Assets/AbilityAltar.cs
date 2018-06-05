using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAltar : MonoBehaviour
{
	[SerializeField] private float distanceThreshold;
	[SerializeField] private float abilityAquireDelay = 1.5f;
	[SerializeField] private GameObject coreParticleSystem;
	[SerializeField] private GameObject siphonParticleSystem;
	[SerializeField] private GameObject explosionParticleSystem;
	[SerializeField] private AudioClip siphonClip;
	[SerializeField] private AudioClip explosionClip;
	[SerializeField] private PlayerLocomotion.PlayerMoves moveToUnlock;

	private AudioSource audioSource;
	private AbilityPowerDraw abilityPowerDraw;
	private PlayerInput playerInput;
	private PlayerLocomotion playerLocomotion;
	private Transform playerTransform;

	private float startTime;
	private bool abilityAwarded = false;
	private bool intialized = false;

	

	private void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		var player = GameObject.FindGameObjectWithTag("Player");
		playerTransform = player.transform;
		playerInput = player.GetComponent<PlayerInput>();
		playerLocomotion = player.GetComponent<PlayerLocomotion>();

		abilityPowerDraw = siphonParticleSystem.GetComponent<AbilityPowerDraw>();
		abilityPowerDraw.SetTarget(playerTransform);
	}

	private void Initialize()
	{
		playerInput.SetHasControl(false);
		startTime = Time.time;
		audioSource.clip = siphonClip;
		audioSource.loop = true;
		audioSource.Play();
		intialized = true;
	}

	private void AwardAbility()
	{
		DisableParticleSystems(coreParticleSystem);
		abilityPowerDraw.DisableEmission();

		PlayAllParticleSystems(explosionParticleSystem);

		audioSource.Stop();
		audioSource.clip = explosionClip;
		audioSource.loop = false;
		audioSource.Play();

		playerInput.SetHasControl(true);
		UnloackAbility();
		abilityAwarded = true;
	}

	private void UnloackAbility()
	{
		switch (moveToUnlock)
		{
			case PlayerLocomotion.PlayerMoves.AirJump:
				playerLocomotion.EnableAirJump();
				break;
			case PlayerLocomotion.PlayerMoves.Dash:
				playerLocomotion.EnableDash();
				break;
			case PlayerLocomotion.PlayerMoves.Glide:
				playerLocomotion.EnableGlide();
				break;
			case PlayerLocomotion.PlayerMoves.WallClimb:
				playerLocomotion.EnableWallClimb();
				break;
			default:
				Debug.LogWarning("Error: Unrecoginized Player Move to unlock!");
				break;
		}
	}

	private void Update ()
	{
		var distanceToPlayer = (transform.position - playerTransform.position).magnitude;
		if (!abilityAwarded && distanceToPlayer < distanceThreshold)
		{
			if (!intialized) { Initialize(); }

			abilityPowerDraw.OnUpdate();

			if (Time.time - startTime > abilityAquireDelay)
			{
				AwardAbility();
			}	
		}
	}

	private void DisableParticleSystems(GameObject gameObject)
	{
		var particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < particleSystems.Length; i++)
		{
			var em = particleSystems[i].emission;
			em.enabled = false;
		}
	}
	private void PlayAllParticleSystems(GameObject gameObject)
	{
		var particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
		for(int i = 0; i < particleSystems.Length ; i++)
		{
			particleSystems[i].Play();
		}
	}
}
