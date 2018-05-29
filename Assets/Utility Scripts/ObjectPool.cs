using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> {
	private Queue<T> objectPoolQueue;
	private int poolSize;

	public ObjectPool (int poolSize)
	{
		objectPoolQueue = new Queue<T>(poolSize);
		this.poolSize = poolSize;
	}

	public void PopulateByInstantiate (GameObject prefab)
	{
		for (int i = 0; i < poolSize; i++)
		{
			Enqueue(GameObject.Instantiate(prefab).GetComponent<T>());
		}
	}

	public T PopWithAutoReque()
	{
		var deQueuedObject = PopWithoutReque();
		objectPoolQueue.Enqueue(deQueuedObject);
		return deQueuedObject;
	}

	public T PopWithoutReque()
	{
		return objectPoolQueue.Dequeue(); ;
	}

	public void Enqueue(T obejctToEnqueue)
	{
		objectPoolQueue.Enqueue(obejctToEnqueue);
	}
}
