using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoailageMovement : MonoBehaviour {
	[SerializeField] private float durration;
	[SerializeField] private Vector3 strength;
	[SerializeField] private int vibrato;
	[SerializeField] private float randomness;

	private Coroutine shakeCoroutine;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(shakeCoroutine != null) { StopCoroutine(shakeCoroutine); }
		shakeCoroutine = StartCoroutine(DoShake());
	}

	private IEnumerator DoShake()
	{
		var tween = transform.DOShakeRotation(durration, strength, vibrato, randomness, true);

		yield return tween.WaitForCompletion();
	}
}
