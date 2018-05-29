using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Health : MonoBehaviour, IDamagable {
	[SerializeField] protected HealthTemplate template;

	// Not Sharred State
	public EventHandler DamageEvent { get; set; }

	private Coroutine deathCorounte;
	private Rigidbody2D rigidbody2d;
	private VFXManager vfxManager;
	private IDamageVisualize graphicsComponent;
	protected AudioSource hurtSoundSource;
	protected float currentHealth;

	private float damageTimer;

	public virtual void Start ()
	{
		hurtSoundSource = GetComponent<AudioSource>();
		rigidbody2d = GetComponent<Rigidbody2D>();
		vfxManager = GameObject.FindGameObjectWithTag("VFX Manager").GetComponent<VFXManager>();
		graphicsComponent = GetComponent<IDamageVisualize>();

		currentHealth = template.MaxHealth;
	}
	
	public virtual void Die()
	{
		if(deathCorounte != null) { return; }
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

	public void Update()
	{
		if(IsInvulnerableTimerRunning())
		{
			damageTimer -= Time.deltaTime;
		}
	}

	public void Damage(float damage, Transform damageDealerTransform)
	{
		if(IsInvulnerableTimerRunning()) { return; }
		
		currentHealth -= damage;

		if(currentHealth <= 0) { Die(); return; }
		damageTimer = template.InvincabilityTime;

		if (graphicsComponent != null) { graphicsComponent.DoDamageVisualize(damageTimer); }
		if (DamageEvent != null) { DamageEvent(null, null); }

		ApplyKnockback(damageDealerTransform);
		PlayHurtAudioCue(hurtSoundSource);
	}

	public bool IsInvulnerableTimerRunning()
	{
		return damageTimer > 0;
	}

	private void ApplyKnockback(Transform damageDealerTransform)
	{
		var dirrectionToDealer = (transform.position - damageDealerTransform.position).normalized;
		var knockBackForce = new Vector2(template.KnockBackPower * dirrectionToDealer.x, template.KnockBackPower);
		// TODO: Decouple Health From applying knockback force from health (?)
		rigidbody2d.AddForce(knockBackForce);
	}
	private void PlayHurtAudioCue(AudioSource audiosource)
	{
		// TODO: Refactor Hurt Sound playing to an event respone (?)
		template.HurtSoundPlayer.Play(audiosource);
	}
}
