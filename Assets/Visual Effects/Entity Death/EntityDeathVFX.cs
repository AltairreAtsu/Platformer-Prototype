using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class EntityDeathVFX : MonoBehaviour, IPlayableVFX
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void PlayVFX(Transform point)
	{
		SetTransformPosition(point);
		animator.SetTrigger("PlayDeathAnim");
	}

	private void SetTransformPosition(Transform point)
	{
		var newPosition = point.position;
		newPosition.y += point.lossyScale.y / 2;

		transform.position = newPosition;
	}
}
