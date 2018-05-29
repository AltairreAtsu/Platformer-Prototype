using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Template", menuName = "Platformer/Health Template")]
public class HealthTemplate : ScriptableObject {
	[SerializeField] private float maxHealth  = 10f;
	[SerializeField] private float knockBackPower = 10f;
	[SerializeField] private AudioPlayer hurtSoundPlayer = null;

	public float MaxHealth { get { return maxHealth; } }
	public float KnockBackPower { get { return knockBackPower; } }
	public AudioPlayer HurtSoundPlayer { get { return hurtSoundPlayer; } }
}
