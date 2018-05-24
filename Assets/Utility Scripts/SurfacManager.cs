using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Surface Manager", menuName ="Platformer/Surface Manager")]
public class SurfacManager : ScriptableObject {
	[SerializeField] private TileSurface[] tileSurfaces;
	[SerializeField] private PhysicsSurface[] physicsSurfaces;

	public ISurfaceType GetSurface(string name)
	{
		for(int i = 0; i < tileSurfaces.Length; i++)
		{
			if (tileSurfaces[i].IsEqual(name))
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
