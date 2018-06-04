using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private SceneMusicManager musicManager;

	[SerializeField] private AudioClip combatTrack;
	[SerializeField] private float sceneChangeTrnasitionTime = 3f;
	[Space]
	[SerializeField] private float cueChangeTrnasitionTime = 3f;
	[Space]
	[SerializeField] private string menuSceneName;

	public static MusicPlayer mainMusicPlayer;

	private AudioSource audioSource;
	private Coroutine transitionCoroutine;
	private float originalAudioVolume;
	

	private void Start ()
	{
		DontDestroyOnLoad(gameObject);
		mainMusicPlayer = this;

		audioSource = GetComponent<AudioSource>();
		originalAudioVolume = audioSource.volume;

		SceneManager.activeSceneChanged += OnSceneChange;
		if(SceneManager.GetActiveScene().name != "Splashscreen")
		{
			audioSource.loop = true;
		}

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
		}
		else
		{
			StartTransition(SceneClip, sceneChangeTrnasitionTime);
		}
	}

	public void TransitionToAggresive()
	{
		if (audioSource.clip == combatTrack) { return; }
		StartTransition(combatTrack, cueChangeTrnasitionTime);
	}
	public void TransitionBackToDefault()
	{
		if (audioSource.clip == GetSceneClipFromManager()) { return; }
		StartTransition(GetSceneClipFromManager(), cueChangeTrnasitionTime);
	}

	public void StartTransition(AudioClip toClip, float time)
	{
		if (transitionCoroutine != null) { StopCoroutine(transitionCoroutine); }
		transitionCoroutine = StartCoroutine(SmoothTransition(toClip, time));
	}
	public void StartTransition(AudioClip toClip, float time, float finalVolume)
	{
		if (transitionCoroutine != null) { StopCoroutine(transitionCoroutine); }
		transitionCoroutine = StartCoroutine(SmoothTransition(toClip, time, finalVolume));
	}


	private void DoSharpTransition(AudioClip SceneClip)
	{
		audioSource.Stop();
		audioSource.clip = SceneClip;
		audioSource.Play();
	}
	private IEnumerator SmoothTransition(AudioClip toClip, float time)
	{
		var tween = audioSource.DOFade(0f, time);
		yield return tween.WaitForCompletion();
		audioSource.clip = toClip;
		audioSource.Play();
		audioSource.DOFade(originalAudioVolume, time);
		yield return tween.WaitForCompletion();
	}
	private IEnumerator SmoothTransition(AudioClip toClip, float time, float finalVolume)
	{
		var tween = audioSource.DOFade(0f, time);
		yield return tween.WaitForCompletion();
		audioSource.clip = toClip;
		audioSource.Play();
		audioSource.DOFade(finalVolume, time);
		yield return tween.WaitForCompletion();
	}

	public void DoTransitionToVolume(float volume, float durration)
	{
		var tween = audioSource.DOFade(volume, durration);
	}

	private void OnDisable()
	{
		SceneManager.activeSceneChanged -= OnSceneChange;
	}
}
