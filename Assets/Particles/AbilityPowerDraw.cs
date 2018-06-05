using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPowerDraw : MonoBehaviour {

	private Transform target;
	private ParticleSystem.EmissionModule emission;
	private ParticleSystem.MainModule main;

	private void Start()
	{
		var particleSystem = GetComponent<ParticleSystem>();
		main = particleSystem.main;
		emission = particleSystem.emission;
		emission.enabled = false;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void OnUpdate()
	{
		var distanceToTarget = (transform.position - target.position).magnitude;

		emission.enabled = true;
		transform.LookAt(target);
		main.startSpeed = distanceToTarget;
	}

	public void DisableEmission()
	{
		emission.enabled = false;
	}

}
