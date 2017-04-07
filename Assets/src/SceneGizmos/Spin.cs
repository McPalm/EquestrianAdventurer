using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
	public float frequency = 1f;

	void Awake()
	{
		Transform = GetComponent<Transform>();
	}

	Transform Transform;

	// Update is called once per frame
	void Update ()
	{
		Transform.Rotate(Vector3.forward, frequency * 360f * Time.deltaTime);
	}
}
