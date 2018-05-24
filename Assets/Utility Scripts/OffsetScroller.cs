using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScroller : MonoBehaviour {
	public float xScrollSpeed;
	private Vector2 savedOffset;

	MeshRenderer meshRenderer;

	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		savedOffset = meshRenderer.sharedMaterial.GetTextureOffset("_MainTex");
	}

	void Update()
	{
		float x = Mathf.Repeat(Time.time * xScrollSpeed, 1);
		Vector2 offset = new Vector2(x, savedOffset.y);
		meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
	}

	void OnDisable()
	{
		meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
	}
}
