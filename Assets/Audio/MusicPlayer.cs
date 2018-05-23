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
	[SerializeField] private float menuTransitionTime = 0.5f;

	private AudioSource audioSource;
	private LevelLoader levelLoader;
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

	public void OnSceneChange(Scene current, Scene next)
	{
		var SceneClip = musicTracks[next.buildIndex];
		if(next.name == menuSceneName)
		{
			DoMainMenuTransition(SceneClip);
		}
		else
		{
			StartTransition(SceneClip, sceneChangeTrnasitionTime, sceneChangeTransitionSteps);
		}
	}

	private void DoMainMenuTransition(AudioClip SceneClip)
	{
		audioSource.clip = SceneClip;
		audioSource.loop = true;
		audioSource.Play();
	}

	public void StartTransition (AudioClip toClip, float time, float steps)
	{
		StopAllCoroutines();
		StartCoroutine(Transition(toClip, time, steps));
	}

	private IEnumerator Transition(AudioClip toClip, float time, float steps)
	{
		var waitTime = time / steps;
		var incriment = originalAudioVolume / steps;

		while (audioSource.volume != 0)
		{
			audioSource.volume = Mathf.Max(audioSource.volume - incriment, 0f);
			yield return new WaitForSeconds(waitTime);
		}
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
}
