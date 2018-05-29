using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeCanvas : MonoBehaviour {
	[SerializeField] private Image fadeImage;
	[SerializeField] private float durration;

	private LevelLoader loader;

	public void Start()
	{
		loader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

		var newColor = fadeImage.color;
		newColor.a = 1f;
		fadeImage.color = newColor;

		DoFadeOut();
	}

	public void DoFadeOut()
	{
		var tween = fadeImage.DOFade(0f, durration);
		tween.SetUpdate(true);
	}

	public void StartFadeInQuit()
	{
		StartCoroutine(DoFadeInQuitCoroutine());
	}
	public void StartFadeInLoad(string sceneName)
	{
		StartCoroutine(DoFadeInLoadScene(sceneName));
	}

	private IEnumerator DoFadeInQuitCoroutine()
	{
		var tween = fadeImage.DOFade(1f, durration);
		tween.SetUpdate(true);
		yield return tween.WaitForCompletion();
		loader.Quit();
	}

	public IEnumerator DoFadeInLoadScene(string sceneName)
	{
		var tween = fadeImage.DOFade(1f, durration);
		tween.SetUpdate(true);
		yield return tween.WaitForCompletion();
		loader.StartLoadingLevel(sceneName);
	}
}
