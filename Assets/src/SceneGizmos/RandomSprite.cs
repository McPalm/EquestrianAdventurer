using UnityEngine;
using System.Collections;

public class RandomSprite : MonoBehaviour
{
	public Sprite[] pool;
	public SpriteRenderer target;

	// Use this for initialization
	void Start ()
	{
		target.sprite = pool[Random.Range(0, pool.Length)];
	}
	
}
