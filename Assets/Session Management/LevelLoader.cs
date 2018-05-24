using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	[SerializeField] private Slider loadingBar;
	[SerializeField] private float splashScreenDelay = 1.5f;
	[SerializeField] private string mainMenuName;

	public float LevelLoadingProgress { get; private set; }
	public bool isDoneLoadingLevel { get; private set; }

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

	public void Quit()
	{
		Application.Quit();
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
			LevelLoadingProgress = Mathf.Clamp01(operation.progress / 0.9f);
			if(loadingBar) loadingBar.value = LevelLoadingProgress;

			yield return null;
		}
	}
}
