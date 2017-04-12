using UnityEngine;
using System.Collections;

public class SfxPool : MonoBehaviour
{
	static SfxPool _instance;

	public AudioSource[] audioSources;

	public static SfxPool Instance
	{
		get
		{
			return _instance;
		}
	}

	int count = 0;

	// Use this for initialization
	void Awake ()
	{
		_instance = this;
	}
	
	public void Play(AudioClip audio, Vector2 location, float volume = 1f)
	{
		AudioSource a = audioSources[count];
		count++;
		count %= audioSources.Length;

		a.transform.position = location;
		a.clip = audio;
		a.volume = volume;
		a.Play();
	}
}
