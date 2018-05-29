using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Template", menuName = "Platformer/Health Template")]
public class HealthTemplate : ScriptableObject {
	[SerializeField] private float maxHealth  = 10f;
	[SerializeField] private float knockBackPower = 10f;
	[SerializeField] private float invicabilityTime;
	[SerializeField] private AudioPlayer hurtSoundPlayer = null;
	[SerializeField] private AudioPlayer deathSoundPlayer = null;

	public float MaxHealth { get { return maxHealth; } }
	public float KnockBackPower { get { return knockBackPower; } }
	public float InvincabilityTime { get { return invicabilityTime; } }
	public AudioPlayer HurtSoundPlayer { get { return hurtSoundPlayer; } }
	public AudioPlayer DeathSoundPlayer { get { return deathSoundPlayer; } }
}
