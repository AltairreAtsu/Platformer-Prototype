using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	[SerializeField] private Slider loadingBar;
	[SerializeField] private float splashScreenDelay = 1.5f;
	[SerializeField] private string mainMenuName;

	public float progress { get; private set; }
	public bool isDone { get; private set; }

	private void Start()
	{
		if(SceneManager.GetActiveScene().buildIndex == 0)
		{
			StartCoroutine(LoadMainMenuFromSplashScreen());
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			StartLoadingLevel("SampleScene");
		}
	}

	public void StartLoadingLevel(string level)
	{
		StartCoroutine(LoadLevel(level));
	}

	private IEnumerator LoadMainMenuFromSplashScreen()
	{
		yield return new WaitForSeconds(splashScreenDelay);
		StartLoadingLevel(mainMenuName);
	}

	private IEnumerator LoadLevel (string Level)
	{
		if (loadingBar) loadingBar.gameObject.SetActive(true); 
		AsyncOperation operation = SceneManager.LoadSceneAsync(Level);

		while (!operation.isDone)
		{
			progress = Mathf.Clamp01(operation.progress / 0.9f);
			if(loadingBar) loadingBar.value = progress;

			yield return null;
		}
	}
}
