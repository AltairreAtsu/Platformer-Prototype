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

	private TrackGroup sceneTrackGroup;
	private string regionKey;
	

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

		SetTrackGroupFromManager();
		audioSource.clip = sceneTrackGroup.GetDefaultClip();
		if (!audioSource.isPlaying) { audioSource.Play(); }
	}


	private void SetTrackGroupFromManager()
	{
		sceneTrackGroup = musicManager.GetTrackGroupFromScene(SceneManager.GetActiveScene().name);
	}
	private AudioClip GetRegionClipFromGroup()
	{
		return sceneTrackGroup.GetClipFromString(regionKey);
	}

	public void OnSceneChange(Scene previous, Scene current)
	{
		SetTrackGroupFromManager();
		var SceneClip = sceneTrackGroup.GetDefaultClip();
		if (current.name == menuSceneName)
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
		if (regionKey == null)
		{
			audioSource.clip = sceneTrackGroup.GetDefaultClip();
			return;
		}
		if (audioSource.clip == GetRegionClipFromGroup()) { return; }
		StartTransition(GetRegionClipFromGroup(), cueChangeTrnasitionTime);
	}

	public void DoRegionTransition(string regionName, float durration)
	{
		if(regionKey == regionName) { return; }
		regionKey = regionName;
		StartTransition(GetRegionClipFromGroup(), durration);
	}
	public void DoRegionTransition(string regionName, float durration, float volume)
	{
		if (regionKey == regionName) { return; }
		regionKey = regionName;
		StartTransition(GetRegionClipFromGroup(), durration, volume);
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
