using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour
{
	[Range(0f, 1f)]
	public float minS;
	[Range(0f, 1f)]
	public float maxS;
	[Range(0.01f, 0.99f)]
	public float minB;
	[Range(0.01f, 0.99f)]
	public float maxB;
	
	

	// Use this for initialization
	void Start ()
	{
		float s = Random.Range(minS, maxS);
		float b = Random.Range(minB, maxB);
		Color c = Color.HSVToRGB(Random.value, s, 0.5f);

		float average = 0.299f * c.r + 0.587f * c.g + 0.114f * c.b;
		c = c * b / average;
		c.a = 1f;


		GetComponent<SpriteRenderer>().color = c;
		
	}
	
}
