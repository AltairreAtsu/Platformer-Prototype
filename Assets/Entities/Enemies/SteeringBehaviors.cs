using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviors : MonoBehaviour {

	public static Vector2 Seek(Vector2 velocity, Vector2 targetPosition, Vector2 selfPosition, float maxVelocity)
	{
		var desiredVelocity = targetPosition - selfPosition;
		desiredVelocity = desiredVelocity.normalized * maxVelocity;
		return desiredVelocity - velocity;
	}

	public static Vector2 Flee(Vector2 velocity, Vector2 targetPosition, Vector2 selfPosition, float maxVelocity)
	{
		var desiredVelocity = selfPosition - targetPosition;
		desiredVelocity = desiredVelocity.normalized * maxVelocity;
		return desiredVelocity - velocity;
	}

	public static Vector2 Arrival(Vector2 velocity, Vector2 targetPosition, Vector2 selfPosition, float maxVelocity, float slowingRadius)
	{
		var desiredVelocity = targetPosition - selfPosition;
		var distance = desiredVelocity.magnitude;

		if (distance < slowingRadius)
		{
			desiredVelocity = desiredVelocity.normalized * maxVelocity * (distance / slowingRadius);
			return desiredVelocity - velocity;
		}
		else
		{
			desiredVelocity = desiredVelocity.normalized * maxVelocity;
			return desiredVelocity - velocity;
		}
	}
}
