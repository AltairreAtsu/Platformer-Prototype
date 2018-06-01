using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialPanel : MonoBehaviour {
	[SerializeField] private GameObject tutorialPanel;
	[SerializeField] private TextMeshProUGUI tutorialText;
	[SerializeField] private float visibileOpacity = 0.9f;
	[SerializeField] private float fadeTimeSeconds = 0.3f;
	[SerializeField] private StringStringDictionary tutorialBlurbsLibrary;

	private Coroutine fadeOutCorountine;
	private Image panelImage;

	private void Start()
	{
		panelImage = tutorialPanel.GetComponent<Image>();
	}

	public void SetTutorialTextByCue(string cue)
	{
		string text = string.Empty;
		tutorialBlurbsLibrary.TryGetValue(cue, out text);

		if(text == string.Empty)
		{
			Debug.LogWarning("Tutorial text Blurb should not be empty! Are you missing a entry for key: " + cue + "?");
		}
		tutorialText.text = text;
	}

	public void FadeIn()
	{
		tutorialPanel.SetActive(true);
		tutorialText.enabled = true;
		panelImage.DOFade(visibileOpacity, fadeTimeSeconds);
	}

	public void StartFadeOut()
	{
		if(fadeOutCorountine != null) { StopCoroutine(FadeOut()); }
		fadeOutCorountine = StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		tutorialText.enabled = false;
		var tween = panelImage.DOFade(0f, fadeTimeSeconds);
		yield return tween.WaitForCompletion();
		tutorialPanel.SetActive(false);
	}
}