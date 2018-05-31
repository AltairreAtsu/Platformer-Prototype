using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (fileName ="New Surface Manager", menuName ="Platformer/Surface Manager")]
public class SurfaceManager : ScriptableObject {
	[SerializeField] private TileSurface[] tileSurfaces;
	[SerializeField] private PhysicsSurface[] physicsSurfaces;

	public ISurfaceType GetSurface(TileBase tile)
	{
		for(int i = 0; i < tileSurfaces.Length; i++)
		{
			if (tileSurfaces[i].IsEqual(tile))
			{
				return tileSurfaces[i];
			}
		}
		return null;
	}

	public ISurfaceType GetSurface(PhysicsMaterial2D material)
	{
		for (int i = 0; i < physicsSurfaces.Length; i++)
		{
			if (physicsSurfaces[i].IsEqual(material))
			{
				return physicsSurfaces[i];
			}
		}
		return null;
	}
}
