using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ScaleCoroutine))]
public class SideSwipe : MonoBehaviour {
	private ScaleCoroutine scaleCoroutine;

	private void Start()
	{
		scaleCoroutine = GetComponent<ScaleCoroutine>();
		GameObject.FindWithTag("Player").GetComponent<PlayerController>().attackEvent += scaleCoroutine.StartScaling;
	}
}
