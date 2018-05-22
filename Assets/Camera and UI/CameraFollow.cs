using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private Transform target;
	private Vector3 DifferentialVector;

	void Start () {
		target = GameObject.FindWithTag("Player").transform;
		DifferentialVector = target.position - transform.position;
	}
	
	void Update () {
		transform.position = target.position - DifferentialVector;		
	}
}
