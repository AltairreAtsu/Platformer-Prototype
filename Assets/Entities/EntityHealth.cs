using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class EntityHealth : StaticHealth
{
	private Rigidbody2D rigidbody2d;

	public override void Start ()
	{
		base.Start();
		rigidbody2d = GetComponent<Rigidbody2D>();
	}
	

	public override void Damage(float damage, Transform damageDealerTransform)
	{
		if (!IsInvulnerableTimerRunning())
		{
			ApplyKnockback(damageDealerTransform);
		}
		base.Damage(damage, damageDealerTransform);
	}


	private void ApplyKnockback(Transform damageDealerTransform)
	{
		var dirrectionToDealer = (transform.position - damageDealerTransform.position).normalized;
		var knockBackForce = new Vector2(template.KnockBackPower * dirrectionToDealer.x, template.KnockBackPower);
		// TODO: Decouple Health From applying knockback force from health (?)
		rigidbody2d.AddForce(knockBackForce);
	}
}
