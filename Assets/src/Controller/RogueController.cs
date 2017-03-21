using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mobile))]
public class RogueController : MonoBehaviour
{

	bool xprio = true;

	Actions actionBuffer;

	float heldDuration = 0f;
	float inputcooldown = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		inputcooldown -= Time.deltaTime;

		if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
		{
			xprio = Input.GetButtonDown("Horizontal");

			heldDuration += Time.deltaTime;
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			if (xprio &&  x < 0f) actionBuffer = Actions.moveleft;
			else if (xprio && x > 0f) actionBuffer = Actions.moveright;
			else if (y < 0f) actionBuffer = Actions.movedown;
			else if (y > 0f) actionBuffer = Actions.moveup;
			else if (x < 0f) actionBuffer = Actions.moveleft;
			else if (x > 0f) actionBuffer = Actions.moveright;
		}
		else if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			heldDuration += Time.deltaTime;
			if (heldDuration > 0.5f && inputcooldown < 0f)
			{
				float x = Input.GetAxis("Horizontal");
				float y = Input.GetAxis("Vertical");
				if (xprio && x < 0f) actionBuffer = Actions.moveleft;
				else if (xprio && x > 0f) actionBuffer = Actions.moveright;
				else if (y < 0f) actionBuffer = Actions.movedown;
				else if (y > 0f) actionBuffer = Actions.moveup;
				else if (x < 0f) actionBuffer = Actions.moveleft;
				else if (x > 0f) actionBuffer = Actions.moveright;
			}
		}
		else
		{
			heldDuration = 0f;
		}

		
		if (actionBuffer != Actions.none && inputcooldown < 0f)
		{
			PerformAction(actionBuffer);
			actionBuffer = Actions.none;
		}
		
	}

	void PerformAction(Actions a)
	{
		if (a == Actions.moveup) Move(new Vector2(0, 1));
		if (a == Actions.moveright) Move(new Vector2(1, 0));
		if (a == Actions.moveleft) Move(new Vector2(-1, 0));
		if (a == Actions.movedown) Move(new Vector2(0, -1));
	}

	void Move(Vector2 where)
	{
		if(GetComponent<Mobile>().MoveDirection(where))
			inputcooldown = 0.19f;
	}

	enum Actions
	{
		none = 0,
		moveup,
		movedown,
		moveleft,
		moveright
	}
}
