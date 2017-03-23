using UnityEngine;
using System.Collections.Generic;

public class TurnTracker : MonoBehaviour
{

	List<SimpleBehaviour> characters;

	// Use this for initialization
	void Start () {
		characters = new List<SimpleBehaviour>(FindObjectsOfType<SimpleBehaviour>());
	}

	float timer = 1f;

	// Update is called once per frame
	void Update ()
	{
		timer -= Time.deltaTime;

		if (timer < 0f)
		{
			foreach (SimpleBehaviour sb in characters)
				sb.DoTurn();
			timer = 1f;
		}
	}
}
