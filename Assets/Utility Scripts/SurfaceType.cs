using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface ISurfaceType
{
	void PlayWalkSound(AudioSource audioSource);
	void PlayLandSound(AudioSource audioSource);
}

[Serializable]
public struct TileSurface : ISurfaceType
{
	[SerializeField] public TileBase tile;
	[SerializeField] public AudioPlayer walkAudioPlayer;
	[SerializeField] public AudioPlayer landAudioPlayer;

	public bool IsEqual(TileBase tile)
	{
		return this.tile == tile; ;
	}

	public void PlayWalkSound(AudioSource audioSource)
	{
		walkAudioPlayer.Play(audioSource);
	}

	public void PlayLandSound(AudioSource audioSource)
	{
		landAudioPlayer.Play(audioSource);
	}
}

[Serializable]
public struct PhysicsSurface : ISurfaceType
{
	[SerializeField] public PhysicsMaterial2D material;
	[SerializeField] public AudioPlayer walkAudioPlayer;
	[SerializeField] public AudioPlayer landAudioPlayer;

	public bool IsEqual(PhysicsMaterial2D material)
	{
		return this.material == material;
	}

	public void PlayWalkSound(AudioSource audioSource)
	{
		walkAudioPlayer.Play(audioSource);
	}

	public void PlayLandSound(AudioSource audioSource)
	{
		landAudioPlayer.Play(audioSource);
	}
}
