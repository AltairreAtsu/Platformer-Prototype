using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(AudioPlayer))]
public class AudioPlayerInspector : Editor {

	SerializedProperty minVolume;
	SerializedProperty maxVolume;
	SerializedProperty minPitch;
	SerializedProperty maxPitch;

	GUIContent testButtonContent;

	private AudioSource TestSource;

	public void OnEnable()
	{
		minVolume = serializedObject.FindProperty("minVolume");
		maxVolume = serializedObject.FindProperty("maxVolume");
		minPitch = serializedObject.FindProperty("minPitch");
		maxPitch = serializedObject.FindProperty("maxPitch");

		testButtonContent = new GUIContent();
		testButtonContent.text = "Play Test Sound";
	}

	public void OnDisable()
	{
		if (TestSource)
		{
			DestroyImmediate(TestSource.gameObject);
		}
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var myTarget = (AudioPlayer)target;
		serializedObject.Update();

		EditorGUILayout.LabelField("Volume Range:");

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField(formatFloat(myTarget.minVolume),
			GUILayout.Width(50f));

		EditorGUILayout.MinMaxSlider(ref myTarget.minVolume, ref myTarget.maxVolume, 0f, 1f,
			GUILayout.MinWidth(230f), GUILayout.MaxWidth(300f));

		EditorGUILayout.LabelField(formatFloat(myTarget.maxVolume),
			GUILayout.Width(50f));

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("Pitch Range:");
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField(formatFloat(myTarget.minPitch), 
			GUILayout.Width(50f));

		EditorGUILayout.MinMaxSlider(ref myTarget.minPitch, ref myTarget.maxPitch, 0f, 3f, 
			GUILayout.MinWidth(230f), GUILayout.MaxWidth(300f));

		EditorGUILayout.LabelField(formatFloat(myTarget.maxPitch),
			GUILayout.Width(50f));

		EditorGUILayout.EndHorizontal();

		minVolume.floatValue = myTarget.minVolume;
		maxVolume.floatValue = myTarget.maxVolume;
		minPitch.floatValue = myTarget.minPitch;
		maxPitch.floatValue = maxPitch.floatValue;

		serializedObject.ApplyModifiedProperties();

		if (EditorGUILayout.DropdownButton(testButtonContent, FocusType.Passive))
		{
			if (!TestSource)
			{
				InstantiateTestSource();
			}
			myTarget.audioSource = TestSource;
			myTarget.Play();
			
		}
	}

	private void InstantiateTestSource()
	{
		TestSource = new GameObject().AddComponent<AudioSource>();
		TestSource.gameObject.hideFlags = HideFlags.HideAndDontSave;
	}

	private string formatFloat(float floatToFormat)
	{
		return string.Format("{0:#.00}", floatToFormat);
	}
}
