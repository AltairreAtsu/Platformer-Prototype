using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Health : MonoBehaviour, IDamagable {
	[SerializeField] private HealthTemplate template;
	[SerializeField] private float invicabilityTime;

	// Not Sharred State
	private AudioSource hurtSoundSource;
	private Rigidbody2D rigidbody2d;
	private VFXManager vfxManager;
	private float currentHealth;

	private float damageTimer;

	private void Start ()
	{
		vfxManager = GameObject.FindGameObjectWithTag("VFX Manager").GetComponent<VFXManager>();
		hurtSoundSource = GetComponent<AudioSource>();
		rigidbody2d = GetComponent<Rigidbody2D>();
		currentHealth = template.MaxHealth;
	}
	
	private void Die()
	{
		vfxManager.SpawnEntityDeathPrefab(transform);
		// TODO: Use Enemy Object Pool
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
		damageTimer = invicabilityTime;

		currentHealth -= damage;

		ApplyKnockback(damageDealerTransform);
		PlayHurtAudioCue(hurtSoundSource);


		if(currentHealth <= 0)
		{
			Die();
		}
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
