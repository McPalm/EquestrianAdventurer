using UnityEngine;
using System.Collections;

public class MovementPredicter : MonoBehaviour
{
	public Mobile target;

	void Start()
	{
		target.EventMovement.AddListener(OnMove);
	}

	Vector2 backlog1;
	Vector2 backlog2;
	Vector2 backlog3;

	float lastUpdate = 0f;
	float idle = 0f;
	public float intensity = 1f;

	void Update()
	{
		idle += Time.deltaTime;
		if(idle > 2f)
		{
			transform.position = target.transform.position;
			enabled = false;
		}
	}

	void OnMove(Vector2 destination, Vector2 direction)
	{
		
		if (idle < 0.2f) idle = 0.2f;
		if (idle > 1.5f)
			intensity = 0.1f;
		else
			intensity = 1f / idle * 0.04f + intensity * 0.8f;

		idle = 0f;
		enabled = true;

		lastUpdate = Time.realtimeSinceStartup;

		transform.position = destination + (direction+ backlog1 + backlog2 + backlog3) * intensity * 0.5f;

		backlog3 = backlog2;
		backlog2 = backlog1;
		backlog1 = direction;
	}
}
