using UnityEngine;
using System.Collections;

public class DragFollow : MonoBehaviour
{
	public GameObject target;
	public float distance = 2f;
	public float smoothing = 0.1f;

	// Update is called once per frame
	void LateUpdate ()
	{
		if(!target)
		{
			enabled = false;
			return;
		}
		Vector3 delta = target.transform.position - transform.position;
		if(delta.x < -distance || delta.x > distance || delta.y < -distance || delta.y > distance)
			transform.position += delta * smoothing;
	}
}
