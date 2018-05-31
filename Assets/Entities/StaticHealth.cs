using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticHealth : MonoBehaviour, IDamagable
{
	[SerializeField] protected HealthTemplate template;
	[SerializeField] protected AudioSource hurtSoundSource;

	public EventHandler DamageEvent { get; set; }

	private Coroutine deathCorounte;
	private IDamageVisualize graphicsComponent;
	private VFXManager vfxManager;
	protected float currentHealth;

	private float damageTimer;

	public virtual void Start()
	{
		vfxManager = GameObject.FindGameObjectWithTag("VFX Manager").GetComponent<VFXManager>();
		graphicsComponent = GetComponent<IDamageVisualize>();

		currentHealth = template.MaxHealth;
	}

	private void Update()
	{
		if (IsInvulnerableTimerRunning())
		{
			damageTimer -= Time.deltaTime;
		}
	}

	public virtual void Damage(float damage, Transform damageDealer)
	{
		if (IsInvulnerableTimerRunning()) { return; }

		currentHealth -= damage;

		if (currentHealth <= 0) { Die(); return; }
		damageTimer = template.InvincabilityTime;

		if (graphicsComponent != null) { graphicsComponent.DoDamageVisualize(damageTimer); }
		if (DamageEvent != null) { DamageEvent(null, null); }

		PlayHurtAudioCue(hurtSoundSource);
	}

	public virtual void Die()
	{
		if (deathCorounte != null) { return; }
		vfxManager.SpawnEntityDeathPrefab(transform);
		template.DeathSoundPlayer.Play(hurtSoundSource);
		// TODO: Use Enemy Object Pool
		deathCorounte = StartCoroutine(DoDestroy(0.1f));
	}

	private IEnumerator DoDestroy(float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}

	public bool IsInvulnerableTimerRunning()
	{
		return damageTimer > 0;
	}

	private void PlayHurtAudioCue(AudioSource audiosource)
	{
		// TODO: Refactor Hurt Sound playing to an event respone (?)
		template.HurtSoundPlayer.Play(audiosource);
	}
}
