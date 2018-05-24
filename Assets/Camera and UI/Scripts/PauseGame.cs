using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PauseGame : MonoBehaviour {
	[SerializeField] private GameObject PausePanel;
	public static bool GAME_PAUSED = false;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (CrossPlatformInputManager.GetButtonDown("Escape"))
		{
			if (GAME_PAUSED)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void SetPanelActive()
	{
		PausePanel.SetActive(true);
	}
	public void SetPanelInactive()
	{
		PausePanel.SetActive(false);
	}

	public void Pause()
	{
		GAME_PAUSED = true;
		Time.timeScale = 0f;
		animator.SetTrigger("SlideIn");
	}

	public void Resume()
	{
		GAME_PAUSED = false;
		Time.timeScale = 1f;
		animator.SetTrigger("SlideOut");
	}

	private void OnDestroy()
	{
		Time.timeScale = 1f;
	}
}
