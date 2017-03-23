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
			if (Input.GetButtonDown("Idle"))
				actionBuffer = Actions.idle;
		}

		
		if (actionBuffer != Actions.none && inputcooldown < 0f)
		{
			PerformAction(actionBuffer);
			actionBuffer = Actions.none;
		}
		
	}

	void PerformAction(Actions a)
	{
		switch (a)
		{
			case Actions.moveup:
				Move(new Vector2(0, 1));
				break;
			case Actions.moveright:
				Move(new Vector2(1, 0));
				break;
			case Actions.moveleft:
				Move(new Vector2(-1, 0));
				break;
			case Actions.movedown:
				Move(new Vector2(0, -1));
				break;
			case Actions.idle:
				EndTurn();
				break;
		}
	}

	void Move(Vector2 where)
	{
		MapCharacter mc = null;
		if (GetComponent<Mobile>().MoveDirection(where, out mc))
		{
			inputcooldown = 0.19f;
			EndTurn();
		}
		if (mc)
		{
			GetComponent<MapCharacter>().Melee(mc);
			EndTurn();
		}

	}

	void EndTurn()
	{
		TurnTracker.Instance.NextTurn();
	}

	enum Actions
	{
		none = 0,
		moveup,
		movedown,
		moveleft,
		moveright,
		idle
	}
}
