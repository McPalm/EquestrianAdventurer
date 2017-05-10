using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Mobile))]
public class CharacterActionController : MonoBehaviour
{
	Stack<Actions> actionStack = new Stack<Actions>();

	public int confusion = 0;
	public int root = 0;

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

		Actions moveAction = Actions.none;

		switch(a)
		{
			case Actions.up:
				didathing = Move(Vector2.up, out moveAction);
				if (moveAction != Actions.movement)
					a = moveAction;
				break;
			case Actions.right:
				didathing = Move(Vector2.right, out moveAction);
				if (moveAction != Actions.movement)
					a = moveAction;
				break;
			case Actions.left:
				didathing = Move(Vector2.left, out moveAction);
				if (moveAction != Actions.movement)
					a = moveAction;
				break;
			case Actions.down:
				didathing = Move(Vector2.down, out moveAction);
				if (moveAction != Actions.movement)
					a = moveAction;
				break;
			case Actions.pickup:
				didathing = inventory.PickupFromGround();
				break;
			default:
				didathing = true;
				break;
		}

		if (!didathing) return false;

		if (root > 0) root--;

		EventAfterAction.Invoke(this, a);

		return true;
	}

	bool Move(Vector2 where, out Actions a)
	{
		MapCharacter mc = null;
		Interactable i = null;
		a = Actions.none;
		if ((root > 0) ? !mobile.BumpDirection(IntVector2.RoundFrom(where), out mc, out i) : mobile.MoveDirection(where, out mc, out i))
		{
			if (root == 0) a = Actions.movement;
			return true;
		}
		else if(mc && mapCharacter.HostileTowards(mc))
		{
			mapCharacter.Melee(mc);
			a = Actions.attack;
			return true;
		}
		else if (canInteract && i)
		{
			
			if(i.Interact(mobile))
			{
				a = Actions.interact;
				return true;
			}
		}
		return false;
	}

	public bool MoveTowards(IntVector2 destination)
	{
		bool moved = true;

		Vector2 delta = (Vector2)destination - (Vector2)mobile.RealLocation;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			if (delta.x < 0f) moved = Perform(Vector2.left);
			else if (delta.x > 0f) moved = Perform(Vector2.right);
			if (!moved)
			{
				if (delta.y < 0f) moved = Perform(Vector2.down);
				else if (delta.y > 0f) moved = Perform(Vector2.up);
				else moved = Perform((UnityEngine.Random.value < 0.5f) ? Vector2.up : Vector2.down);
			}

		}
		else if (delta.y != 0f)
		{

			if (delta.y < 0f) moved = Perform(Vector2.down);
			else if (delta.y > 0f) moved = Perform(Vector2.up);
			if (!moved)
			{
				if (delta.x < 0f) moved = Perform(Vector2.left);
				else if (delta.x > 0f) moved = Perform(Vector2.right);
				else moved = Perform((UnityEngine.Random.value < 0.5f) ? Vector2.left : Vector2.right);
			}
		}

		return moved;
	}

	public bool MoveAway(IntVector2 destination)
	{
		bool moved = true;

		Vector2 delta = (Vector2)destination - (Vector2)mobile.RealLocation;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			if (delta.x < 0f) moved = Perform(Vector2.right);
			else if (delta.x > 0f) moved = Perform(Vector2.left);
			if (!moved)
			{
				if (delta.y < 0f) moved = Perform(Vector2.up);
				else if (delta.y > 0f) moved = Perform(Vector2.down);
				else moved = Perform((UnityEngine.Random.value < 0.5f) ? Vector2.down : Vector2.up);
			}

		}
		else if (delta.y != 0f)
		{

			if (delta.y < 0f) moved = Perform(Vector2.up);
			else if (delta.y > 0f) moved = Perform(Vector2.down);
			if (!moved)
			{
				if (delta.x < 0f) moved = Perform(Vector2.right);
				else if (delta.x > 0f) moved = Perform(Vector2.left);
				else moved = Perform((UnityEngine.Random.value < 0.5f) ? Vector2.right : Vector2.left);
			}
		}

		return moved;
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
		inventoryaction = 0x40,
		attack = 0x80,
		interact = 0x100,
		ability = 0x200,
	}
}
