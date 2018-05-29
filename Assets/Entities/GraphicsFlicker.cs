using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsFlicker : MonoBehaviour, IDamageVisualize {
	[SerializeField] private SpriteRenderer spriteRender;
	[SerializeField] private int flickers;

	private Coroutine graphicsFlickerCorounte;

	public void DoDamageVisualize(float durration)
	{
		StartGraphicsFlicker(durration, flickers);
	}

	private void StartGraphicsFlicker(float durration, int flickers)
	{
		if (graphicsFlickerCorounte != null) { StopGraphicsFlicker(); }
		graphicsFlickerCorounte = StartCoroutine(DoGraphicsFlicker(durration, flickers));
	}

	public void StopGraphicsFlicker()
	{
		StopCoroutine(graphicsFlickerCorounte);
	}

	private IEnumerator DoGraphicsFlicker(float durration, int flickers)
	{
		var segmentDuration = durration / flickers;
		var startTime = Time.time;

		while (Time.time - startTime < durration)
		{
			spriteRender.enabled = !spriteRender.enabled;
			yield return new WaitForSeconds(segmentDuration);
		}
		spriteRender.enabled = true;
	}
}
