using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private AudioClip[] musicTracks;
	[SerializeField] private float sceneChangeTrnasitionTime = 3f;
	[SerializeField] private float sceneChangeTransitionSteps = 10f;
	[SerializeField] private string menuSceneName;

	private AudioSource audioSource;
	private float originalAudioVolume;

	private void Start ()
	{
		DontDestroyOnLoad(gameObject);
		audioSource = GetComponent<AudioSource>();
		originalAudioVolume = audioSource.volume;

		SceneManager.activeSceneChanged += OnSceneChange;

		audioSource.clip = musicTracks[SceneManager.GetActiveScene().buildIndex];
		if (!audioSource.isPlaying) { audioSource.Play(); }
	}

	public void OnSceneChange(Scene previous, Scene current)
	{
		var SceneClip = musicTracks[current.buildIndex];
		if(current.name == menuSceneName)
		{
			DoSharpTransition(SceneClip);
			audioSource.loop = true;
		}
		else
		{
			StartTransition(SceneClip, sceneChangeTrnasitionTime, sceneChangeTransitionSteps);
			print("Smoothly transition!");
		}
	}

	private void DoSharpTransition(AudioClip SceneClip)
	{
		audioSource.Stop();
		audioSource.clip = SceneClip;
		audioSource.Play();
	}

	private IEnumerator SmoothTransition(AudioClip toClip, float time, float steps)
	{
		var waitTime = time / steps;
		var incriment = originalAudioVolume / steps;

		while (audioSource.volume != 0)
		{
			audioSource.volume = Mathf.Max(audioSource.volume - incriment, 0f);
			yield return new WaitForSeconds(waitTime);
		}

		audioSource.Stop();
		audioSource.clip = toClip;
		audioSource.Play();

		while (audioSource.volume != originalAudioVolume)
		{
			audioSource.volume = Mathf.Min(audioSource.volume + incriment, originalAudioVolume);
			yield return new WaitForSeconds(waitTime);
		}
	}

	private void OnDisable()
	{
		SceneManager.activeSceneChanged -= OnSceneChange;
	}

	public void StartTransition(AudioClip toClip, float time, float steps)
	{
		StopAllCoroutines();
		StartCoroutine(SmoothTransition(toClip, time, steps));
	}
}
