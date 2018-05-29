using UnityEngine;
using System;

public interface IDamagable {
	EventHandler DamageEvent { get; set; }
	void Die();
	void Damage(float damage, Transform damageDealer);
}

public interface IDamageVisualize
{
	void DoDamageVisualize(float durration);
}