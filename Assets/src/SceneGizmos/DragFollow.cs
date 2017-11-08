using UnityEngine;
using System.Collections;

public class DragFollow : MonoBehaviour
{
	public GameObject target;
	public float distance = 2f;
	public float smoothing = 0.1f;
	public float pause = 0f;

    public bool constrained = true;
    public Vector2 max;
    public Vector2 min;


	// Update is called once per frame
	void LateUpdate ()
	{
		if(pause > 0f)
		{
			pause -= Time.deltaTime;
			return;
		}
		if(!target)
		{
			enabled = false;
			return;
		}
		Vector3 delta = target.transform.position - transform.position;
		if(delta.x < -distance || delta.x > distance || delta.y < -distance || delta.y > distance)
			transform.position += delta * smoothing;
        if(constrained)
        {
            transform.position = new Vector3(Mathf.Max(min.x, transform.position.x), Mathf.Max(min.y, transform.position.y), transform.position.z);
            transform.position = new Vector3(Mathf.Min(max.x, transform.position.x), Mathf.Min(max.y, transform.position.y), transform.position.z);
        }
	}

	public void Pause(float seconds)
	{
		pause = seconds;
	}
}
