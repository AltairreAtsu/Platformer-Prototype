using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;



[CreateAssetMenu (fileName = "New Scene Music Manager", menuName ="Platformer/SceneMusicManager")]
public class SceneMusicManager : ScriptableObject {
	[SerializeField] StringAudioClipDictionary SceneMusicLibrary;

	public AudioClip GetClipFromSceneName(string name)
	{
		AudioClip clip = null;
		SceneMusicLibrary.TryGetValue(name, out clip);
		if(clip == null)
		{
			Debug.LogError("Music Player Could not find clip for Scene: " + name);
		}

		return clip;
	}
}

[Serializable]
public class StringAudioClipDictionary : SerializableDictionary<string, AudioClip> { }

