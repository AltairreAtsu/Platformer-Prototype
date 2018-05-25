using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionMockUp : MonoBehaviour {

	[SerializeField] private GameObject musicPlayerPrefab;

	void Start () {
		var musicPlayer = FindObjectOfType<MusicPlayer>();
		if(musicPlayer == null)
		{
			Instantiate(musicPlayerPrefab);
		}

		Destroy(gameObject);
	}
	
}
