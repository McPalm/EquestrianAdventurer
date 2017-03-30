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

	public bool Perform(Vector2 d)
	{
		if (d == Vector2.right) return Perform(Actions.right);
		if (d == Vector2.left) return Perform(Actions.left);
		if (d == Vector2.up) return Perform(Actions.up);
		if (d == Vector2.down) return Perform(Actions.down);
		return false;
	}

	public bool Perform(Actions a)
	{
		bool didathing = false;

		if (HasStackedAction)
			a = actionStack.Pop();

		if(confusion > 0) // when confused, there is a 33% chance to do a random action instead
		{
			switch(Random.Range(0, 12))
			{
				case 0: a = Actions.up; break;
				case 1: a = Actions.down; break;
				case 2: a = Actions.left; break;
				case 3: a = Actions.right; break;
			}
			confusion--;
		}

		EventBeforeAction.Invoke(this, a);

		switch(a)
		{
			case Actions.up:
				didathing = Move(Vector2.up);
				break;
			case Actions.right:
				didathing = Move(Vector2.right);
				break;
			case Actions.left:
				didathing = Move(Vector2.left);
				break;
			case Actions.down:
				didathing = Move(Vector2.down);
				break;
			case Actions.pickup:
				didathing = inventory.PickupFromGround();
				break;
			default:
				didathing = true;
				break;
		}

		EventAfterAction.Invoke(this, a);

		return didathing;
	}

	bool Move(Vector2 where)
	{
		MapCharacter mc = null;
		if (mobile.MoveDirection(where, out mc))
		{
			return true;
		}
		else if(mc)
		{
			Interactable i = mc.GetComponent<Interactable>();
			if(mapCharacter.HostileTowards(mc))
			{
				mapCharacter.Melee(mc);
				return true;
			}
			else if (canInteract && i)
			{
				return i.Interact(mobile);
			}
		}
		return false;
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
