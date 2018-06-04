using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Treack Group", menuName ="Platformer/Track Group")]
public class TrackGroup : ScriptableObject {
	[SerializeField] private AudioClip defaultTrack;
	[SerializeField] private StringAudioClipDictionary regionClips;

	public AudioClip GetClipFromString(string key)
	{
		AudioClip clip;
		regionClips.TryGetValue(key, out clip);
		if(clip == null) { Debug.LogWarning("No value found for region name: " + key + " are you missing a key value pair?"); }
		return clip;
	}

	public AudioClip GetDefaultClip()
	{
		return defaultTrack;
	}
}

[System.Serializable]
public class StringAudioClipDictionary : SerializableDictionary<string, AudioClip> { }
