using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health {
	[SerializeField] private PlayerSettings playerSettings;

	private PlayerController playerController;
	private Animator animator;

	public delegate void PlayerEvent();
	public event PlayerEvent respawnEvent;
	public event PlayerEvent dieEvent;

	public override void Start()
	{
		base.Start();
		playerController = GetComponent<PlayerController>();
		animator = GetComponent<Animator>();
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(playerSettings.RespawnTimeSeconds);
		if (respawnEvent != null) { respawnEvent(); }

		animator.SetBool("Dead", false);
		playerController.TeleportToCheckpoint();
		playerController.SetCollidersActiveState(true);
		currentHealth = template.MaxHealth;
	}

	public override void Die()
	{
		if(animator.GetBool("Dead")) { return; }
		if (dieEvent != null) { dieEvent(); }
		template.DeathSoundPlayer.Play(hurtSoundSource);
		animator.SetBool("Dead", true);
		playerController.SetCollidersActiveState(false);

		StartCoroutine(Respawn());
	}
}
