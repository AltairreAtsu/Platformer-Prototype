using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private Vector2 startPoint;
	[SerializeField] private Vector2 endPoint;
	[SerializeField] private float durration;
	[SerializeField] private float easePeriod;
	[SerializeField] private float pointDelay;

	private Vector2 originalPosition;

	private void Start ()
	{
		originalPosition = transform.position;
		GoToEndPoint();
	}
	
	private void GoToEndPoint()
	{
		var tween = transform.DOMove(endPoint + originalPosition, durration);
		tween.easePeriod = easePeriod;
		tween.SetDelay(pointDelay);
		tween.onComplete += GoToStartPoint;
	}

	private void GoToStartPoint()
	{
		var tween = transform.DOMove(startPoint + originalPosition, durration);
		tween.easePeriod = easePeriod;
		tween.SetDelay(pointDelay);
		tween.onComplete += GoToEndPoint;
	}

	private void OnDrawGizmos()
	{
		if(originalPosition != (Vector2)transform.position && !Application.isPlaying)
		{
			originalPosition = transform.position;
		}

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(startPoint + originalPosition, endPoint + originalPosition);
		Gizmos.DrawSphere(startPoint + originalPosition, 0.2f);
		Gizmos.DrawSphere(endPoint + originalPosition, 0.2f);
	}
}
