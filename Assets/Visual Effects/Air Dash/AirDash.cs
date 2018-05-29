using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDash : MonoBehaviour {

	private ScaleCoroutine scaleCoroutine;

	private void Start()
	{
		scaleCoroutine = GetComponent<ScaleCoroutine>();
		GameObject.FindWithTag("Player").GetComponent<PlayerLocomotion>().dashEvent += scaleCoroutine.StartScaling;
	}
}
