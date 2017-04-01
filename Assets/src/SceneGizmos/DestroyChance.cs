using UnityEngine;
using System.Collections;

public class DestroyChance : MonoBehaviour
{
	[Range(0f, 1f)]
	public float chance;


	// Use this for initialization
	void Awake()
	{
		if (Random.value < chance)
			gameObject.SetActive(false);
	}

}
