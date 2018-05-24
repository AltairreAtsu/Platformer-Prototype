using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	private Transform target;
	private Vector3 DifferentialVector;

	private bool followingPlayer = true;

	private void Start ()
	{
		target = GameObject.FindWithTag("Player").transform;
		DifferentialVector = target.position - transform.position;

		var playerMovement = target.GetComponent<PlayerMovement>();
		playerMovement.respawnEvent += StartFollowingPlayer;
		playerMovement.dieEvent += StopFollowingPlayer;
	}
	
	private void Update ()
	{
		if (followingPlayer)
		{
			transform.position = target.position - DifferentialVector;
		}
	}

	public void StopFollowingPlayer()
	{
		followingPlayer = false;
	}
	public void StartFollowingPlayer()
	{
		followingPlayer = true;
	}
}
