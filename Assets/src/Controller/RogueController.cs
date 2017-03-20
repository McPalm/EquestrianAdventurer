using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mobile))]
public class RogueController : MonoBehaviour
{



	float inputcooldown = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(inputcooldown > 0f)
		{
			inputcooldown -= Time.deltaTime;
			return;
		}
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		if (x < 0f) Move(new Vector2(-1f, 0f));
		else if (x > 0f) Move(new Vector2(1f, 0f));
		else if (y < 0f) Move(new Vector2(0f, -1f));
		else if (y > 0f) Move(new Vector2(0f, 1f));
	}

	void Move(Vector2 where)
	{
		print(where);
		GetComponent<Mobile>().MoveTo((Vector2)transform.position + where);
		EndTurn();
	}

	void EndTurn()
	{
		inputcooldown = 0.19f;
	}
}
