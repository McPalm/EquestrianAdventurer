using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour
{

	public AudioClip[] clip;

	public void Play()
	{
		SfxPool.Instance.Play(clip[Random.Range(0, clip.Length)], transform.position, 1f);
	}

	public void Play(float volume)
	{
		SfxPool.Instance.Play(clip[Random.Range(0, clip.Length)], transform.position, volume);
	}
}
