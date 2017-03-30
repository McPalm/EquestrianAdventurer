using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterActionController))]
public class RogueController : MonoBehaviour
{

	bool xprio = true;

	CharacterActionController.Actions actionBuffer;

	float heldDuration = 0f;
	float inputcooldown = 0f;

	CharacterActionController controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterActionController>();
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
			if (xprio &&  x < 0f) actionBuffer = CharacterActionController.Actions.left;
			else if (xprio && x > 0f) actionBuffer = CharacterActionController.Actions.right;
			else if (y < 0f) actionBuffer = CharacterActionController.Actions.down;
			else if (y > 0f) actionBuffer = CharacterActionController.Actions.up;
			else if (x < 0f) actionBuffer = CharacterActionController.Actions.left;
			else if (x > 0f) actionBuffer = CharacterActionController.Actions.right;
		}
		else if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			heldDuration += Time.deltaTime;
			if (heldDuration > 0.5f && inputcooldown < 0f)
			{
				float x = Input.GetAxis("Horizontal");
				float y = Input.GetAxis("Vertical");
				if (xprio && x < 0f) actionBuffer = CharacterActionController.Actions.left;
				else if (xprio && x > 0f) actionBuffer = CharacterActionController.Actions.right;
				else if (y < 0f) actionBuffer = CharacterActionController.Actions.down;
				else if (y > 0f) actionBuffer = CharacterActionController.Actions.up;
				else if (x < 0f) actionBuffer = CharacterActionController.Actions.left;
				else if (x > 0f) actionBuffer = CharacterActionController.Actions.right;
			}
		}
		else
		{
			heldDuration = 0f;
			if (Input.GetButtonDown("Idle"))
				actionBuffer = CharacterActionController.Actions.idle;
			if (Input.GetButtonDown("Pickup"))
				actionBuffer = CharacterActionController.Actions.pickup;
		}

		
		if (controller.HasStackedAction || actionBuffer != CharacterActionController.Actions.none && inputcooldown < 0f)
		{
			controller.Perform(actionBuffer);
			if (actionBuffer == CharacterActionController.Actions.idle)
				inputcooldown = 0.8f;
			else
				inputcooldown = 0.19f;
			actionBuffer = CharacterActionController.Actions.none;
			EndTurn();
		}
	}

	void EndTurn()
	{
		TurnTracker.Instance.NextTurn();
	}
}
