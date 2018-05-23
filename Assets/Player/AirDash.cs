using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDash : MonoBehaviour {

	[SerializeField] private SpriteRenderer airDashSpriteRender;
	[SerializeField] private float maxXScale = 1.5f;
	[SerializeField] private float startXScale = 1f;
	[SerializeField] private float scaleStep = 0.1f;
	[SerializeField] private float timeStepSeconds = 0.1f;

	private void Start()
	{
		GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().dashEvent += StartDash;
	}

	private void StartDash()
	{
		StartCoroutine(DashCoroutine());
	}

	private IEnumerator DashCoroutine ()
	{
		airDashSpriteRender.enabled = true;
		transform.localScale = new Vector3(startXScale, 1f, 1f);

		var scaleVector = transform.localScale;

		while (transform.localScale.x < maxXScale)
		{
			scaleVector.x = transform.localScale.x + scaleStep;
			transform.localScale = scaleVector;
			yield return new WaitForSeconds(timeStepSeconds);
		}

		airDashSpriteRender.enabled = false;
	}
	
}
