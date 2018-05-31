using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private SceneMusicManager musicManager;

	[SerializeField] private AudioClip combatTrack;
	[SerializeField] private float sceneChangeTrnasitionTime = 3f;
	[SerializeField] private float sceneChangeTransitionSteps = 10f;
	[Space]
	[SerializeField] private float cueChangeTrnasitionTime = 3f;
	[SerializeField] private float cueChangeTransitionSteps = 10f;
	[Space]
	[SerializeField] private string menuSceneName;

	public static MusicPlayer mainMusicPlayer;

	private AudioSource audioSource;
	private float originalAudioVolume;

	private void Start ()
	{
		DontDestroyOnLoad(gameObject);
		mainMusicPlayer = this;

		audioSource = GetComponent<AudioSource>();
		originalAudioVolume = audioSource.volume;

		SceneManager.activeSceneChanged += OnSceneChange;

		audioSource.clip = GetSceneClipFromManager();
		if (!audioSource.isPlaying) { audioSource.Play(); }
	}

	private AudioClip GetSceneClipFromManager()
	{
		return musicManager.GetClipFromSceneName(SceneManager.GetActiveScene().name);
	}

	public void OnSceneChange(Scene previous, Scene current)
	{
		var SceneClip = GetSceneClipFromManager();
		if(current.name == menuSceneName)
		{
			DoSharpTransition(SceneClip);
			audioSource.loop = true;
		}
		else
		{
			StartTransition(SceneClip, sceneChangeTrnasitionTime, sceneChangeTransitionSteps);
		}
	}

	public void TransitionToAggresive()
	{
		if (audioSource.clip == combatTrack) { return; }
		StartTransition(combatTrack, cueChangeTrnasitionTime, cueChangeTransitionSteps);
	}
	public void TransitionBackToDefault()
	{
		if (audioSource.clip == GetSceneClipFromManager()) { return; }
		StartTransition(GetSceneClipFromManager(), cueChangeTrnasitionTime, cueChangeTransitionSteps);
	}

	public void StartTransition(AudioClip toClip, float time, float steps)
	{
		StopAllCoroutines();
		StartCoroutine(SmoothTransition(toClip, time, steps));
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
}
