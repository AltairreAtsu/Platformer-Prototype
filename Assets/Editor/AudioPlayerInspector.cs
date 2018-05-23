using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(AudioPlayer))]
public class AudioPlayerInspector : Editor {

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var myTarget = (AudioPlayer)target;

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

		
	}

	public string formatFloat(float floatToFormat)
	{
		return string.Format("{0:#.00}", floatToFormat);
	}
}
