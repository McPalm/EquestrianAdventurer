using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// give it a target and it will move to the target and bump it.
/// </summary>
[RequireComponent(typeof(CharacterActionController))]
public class SimpleBehaviour : MonoBehaviour, TurnTracker.TurnEntry
{
	public MapCharacter targetCharacter;
	public IntVector2 targetLocation;

	CharacterActionController controller;

	/// <summary>
	/// use this to modify target and targetlocation
	/// </summary>
	public StartTurnEvent startTurnEvent = new StartTurnEvent();
	public StartTurnEvent endTurnEvent = new StartTurnEvent();

	/// <summary>
	/// called when the AI is given an action
	/// </summary>
	public void DoTurn()
	{
		startTurnEvent.Invoke(this);

		Vector2 delta = (Vector3)targetLocation - transform.position;
		if(targetCharacter) delta = targetCharacter.transform.position - transform.position;

		bool moved = true;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			if (delta.x < 0f) moved = controller.Perform(Vector2.left);
			else if (delta.x > 0f) moved = controller.Perform(Vector2.right);
			if(!moved)
			{
				if (delta.y < 0f) moved = controller.Perform(Vector2.down);
				else if (delta.y > 0f) moved = controller.Perform(Vector2.up);
				else moved = controller.Perform((Random.value < 0.5f) ? Vector2.up : Vector2.down);
			}

		}
		else if(delta.y != 0f)
		{
			
			if (delta.y < 0f) moved = controller.Perform(Vector2.down);
			else if (delta.y > 0f) moved = controller.Perform(Vector2.up);
			if (!moved)
			{
				if (delta.x < 0f) moved = controller.Perform(Vector2.left);
				else if (delta.x > 0f) moved = controller.Perform(Vector2.right);
				else moved = controller.Perform((Random.value < 0.5f) ? Vector2.left : Vector2.right);
			}
		}

		if (!moved) controller.Perform(CharacterActionController.Actions.idle);

		endTurnEvent.Invoke(this);
	}

	void Start()
	{
		controller = GetComponent<CharacterActionController>();
		TurnTracker.Instance.Add(this);
		targetLocation = IntVector2.RoundFrom(transform.position);
		GetComponent<MapCharacter>().EventDeath.AddListener(delegate { TurnTracker.Instance.Remove(this); });
	}

	void OnApplicationQuit()
	{
		teardown = true;
	}
	bool teardown = false;
	void OnDisable()
	{
		if (teardown) return;
		TurnTracker.Instance.Remove(this);
	}

	public class StartTurnEvent : UnityEvent<SimpleBehaviour> { }
}
