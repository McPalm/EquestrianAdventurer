using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Mobile))]
public class CharacterActionController : MonoBehaviour
{
	Stack<Actions> actionStack = new Stack<Actions>();

	public int confusion = 0;

	public CharacterActionControllerEvent EventBeforeAction = new CharacterActionControllerEvent();
	public CharacterActionControllerEvent EventAfterAction = new CharacterActionControllerEvent();


	Mobile mobile;
	MapCharacter mapCharacter;
	Inventory inventory;
	bool canInteract;

	void Start()
	{
		mobile = GetComponent<Mobile>();
		mapCharacter = GetComponent<MapCharacter>();
		inventory = GetComponent<Inventory>();
		canInteract = GetComponent<RogueController>(); // due to MonoBehaviours refferese being implicity castable to a bool. null = false
	}

	public bool HasStackedAction
	{
		get
		{
			return actionStack.Count > 0;
		}
	}

	public void StackAction(Actions a)
	{
		actionStack.Push(a);
	}

	public void Perform(Actions a)
	{

		if (HasStackedAction)
			a = actionStack.Pop();

		if(confusion > 0) // when confused, there is a 33% chance to do a random action instead
		{
			if(Random.value < 0.33f)
				a = (Actions)Random.Range(0, 5);
			confusion--;
		}

		EventBeforeAction.Invoke(this, a);

		switch(a)
		{
			case Actions.up:
				Move(Vector2.up);
				break;
			case Actions.right:
				Move(Vector2.right);
				break;
			case Actions.left:
				Move(Vector2.left);
				break;
			case Actions.down:
				Move(Vector2.down);
				break;
			case Actions.pickup:
				inventory.PickupFromGround();
				break;
			default:
				break;
		}

		EventAfterAction.Invoke(this, a);
	}

	void Move(Vector2 where)
	{
		MapCharacter mc = null;
		if (mobile.MoveDirection(where, out mc))
		{
			return;
		}
		else if(mc)
		{
			Interactable i = mc.GetComponent<Interactable>();
			if (canInteract && i)
			{
				i.Interact(mobile);
			}
			else
			{
				mapCharacter.Melee(mc);
			}
		}

	}

	[System.Serializable]
	public class CharacterActionControllerEvent : UnityEvent<CharacterActionController, Actions> { }

	public enum Actions
	{
		none = 0x0,
		idle = 0x1,
		up = 0x2,
		down = 0x4,
		left = 0x8,
		right = 0x10,
		movement = 0x1E, // 2 + 4 + 8 + 16 = 30 = 1E
		pickup = 0x20,
		inventoryaction = 0x40
	}
}
