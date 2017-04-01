using UnityEngine;
using System.Collections;

public class FutzLocation : MonoBehaviour
{
	public float minX = -0.5f;
	public float minY = -0.5f;
	public float maxX = 0.5f;
	public float maxY = 0.5f;


	// Use this for initialization
	void Start ()
	{
		transform.position += new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
	}
}
