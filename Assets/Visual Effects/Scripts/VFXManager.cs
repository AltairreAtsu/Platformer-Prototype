using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {
	[SerializeField] private GameObject entityDeathPrefab;
	[SerializeField] private int entityDeathPoolSize;

	private ObjectPool<IPlayableVFX> entityDeathPool;

	private void Start()
	{
		entityDeathPool = new ObjectPool<IPlayableVFX>(entityDeathPoolSize);
		entityDeathPool.PopulateByInstantiate(entityDeathPrefab);
	}

	public void SpawnEntityDeathPrefab (Transform point)
	{
		var entityDeathVFX = entityDeathPool.PopWithAutoReque();
		entityDeathVFX.PlayVFX(point);
	}
}
