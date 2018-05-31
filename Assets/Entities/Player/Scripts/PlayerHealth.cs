using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth {
	[SerializeField] private PlayerSettings playerSettings;
	[Header ("Events")]
	[SerializeField] private GameEvent playerDieEvent;
	[SerializeField] private GameEvent playerRespawnEvent;

	private PlayerController playerController;
	private Animator animator;

	public override void Start()
	{
		base.Start();
		playerController = GetComponent<PlayerController>();
		animator = GetComponent<Animator>();
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(playerSettings.RespawnTimeSeconds);
		playerRespawnEvent.Raise();

		animator.SetBool("Dead", false);
		playerController.TeleportToCheckpoint();
		playerController.SetCollidersActiveState(true);
		currentHealth = template.MaxHealth;
	}

	public override void Die()
	{
		if(animator.GetBool("Dead")) { return; }
		playerDieEvent.Raise();
		template.DeathSoundPlayer.Play(hurtSoundSource);
		animator.SetBool("Dead", true);
		playerController.SetCollidersActiveState(false);

		StartCoroutine(Respawn());
	}
}
