using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterActionController))]
public class RogueController : MonoBehaviour
{

	bool xprio = true;

	CharacterActionController.Actions actionBuffer;

	float heldDuration = 0f;
	float inputcooldown = 0f;
	IntVector2 clickLocation;

	CharacterActionController controller;

	public AActiveAbility SelectedAbility;

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
			if (bufferedMovementOrNothing())
			{
				xprio = Input.GetButtonDown("Horizontal");

				heldDuration += Time.deltaTime;
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
		else if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			if (bufferedMovementOrNothing())
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
		}
		else
		{
			heldDuration = 0f;
			if (Input.GetButtonDown("Idle"))
				actionBuffer = CharacterActionController.Actions.idle;
			if (Input.GetButtonDown("Pickup"))
				actionBuffer = CharacterActionController.Actions.pickup;
		}

		if (Input.GetMouseButtonDown(1) & !EventSystem.current.IsPointerOverGameObject()) // exectue buffered rightclick
		{
			actionBuffer = CharacterActionController.Actions.ability;
			clickLocation = IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
		if (controller.HasStackedAction && inputcooldown > 0.08f) inputcooldown = 0.08f; // if having stacked action, reduce the inputcooldown down to 0.08 seconds
		if (actionBuffer == CharacterActionController.Actions.ability && inputcooldown < 0f)
		{
			if (SelectedAbility)
			{
				if (SelectedAbility.TryUseAt(clickLocation))
				{
					controller.Perform(CharacterActionController.Actions.ability); // TODO maybe. I dont know.
					inputcooldown = 0.19f;
					EndTurn();
				}
			}
			actionBuffer = CharacterActionController.Actions.none;
		}
		else if ((controller.HasStackedAction || actionBuffer != CharacterActionController.Actions.none) && inputcooldown < 0f)
		{
			if (controller.Perform(actionBuffer))
			{
				if (actionBuffer == CharacterActionController.Actions.idle)
					inputcooldown = 0.08f;
				else
					inputcooldown = 0.19f;
				EndTurn();
			}
			else
				inputcooldown = 0.1f;
			actionBuffer = CharacterActionController.Actions.none;
		}
	}

	void EndTurn()
	{
		actionBuffer = CharacterActionController.Actions.none;
		TurnTracker.Instance.NextTurn();
	}

	bool bufferedMovementOrNothing()
	{
		if (actionBuffer == CharacterActionController.Actions.none) return true;
		return (actionBuffer & CharacterActionController.Actions.movement) > 0;
	}

	public AActiveAbility[] Abilities
	{
		get
		{
			return GetComponentsInChildren<AActiveAbility>();
		}
	}
}
