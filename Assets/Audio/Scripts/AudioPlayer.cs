using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Player", menuName = "Platformer/Audio Player")]
public class AudioPlayer : ScriptableObject
{
	[SerializeField] private AudioClip[] clips;

	[HideInInspector] [SerializeField] public float minPitch = -1;
	[HideInInspector] [SerializeField] public float maxPitch = 3;
	[HideInInspector] [SerializeField] public float maxVolume = 1;
	[HideInInspector] [SerializeField] public float minVolume = 0;

	public void Play(AudioSource audioSource)
	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.volume = Random.Range(minVolume, maxVolume);

		var clip = (clips.Length > 1) ? clips[Random.Range(0, clips.Length - 1)] : clips[0];
		audioSource.PlayOneShot(clip);
	}

}
