using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[CreateAssetMenu (fileName = "New Scene Music Manager", menuName ="Platformer/SceneMusicManager")]
public class SceneMusicManager : ScriptableObject {
	[SerializeField] StringTrackGroupDictionary SceneMusicLibrary;

	public TrackGroup GetTrackGroupFromScene(string key)
	{
		TrackGroup group;
		SceneMusicLibrary.TryGetValue(key, out group);
		if(group == null) { Debug.LogWarning("No Track group found for scene name " + key + " are you missing a key value pair?"); }
		return group;
	}
}

[System.Serializable]
public class StringTrackGroupDictionary : SerializableDictionary<string, TrackGroup> { }

