using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// give it a target and it will move to the target and bump it.
/// </summary>
[RequireComponent(typeof(MapCharacter))]
public class SimpleBehaviour : MonoBehaviour, TurnTracker.TurnEntry
{
	public MapCharacter targetCharacter;
	public IntVector2 targetLocation;

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

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			if (delta.x < 0f) MoveDirection(Vector2.left);
			else if (delta.x > 0f) MoveDirection(Vector2.right);
		}
		else if(delta.y != 0f)
		{
			if (delta.y < 0f) MoveDirection(Vector2.down);
			else if (delta.y > 0f) MoveDirection(Vector2.up);
		}

		endTurnEvent.Invoke(this);
	}

	void MoveDirection(Vector2 v2)
	{
		MapCharacter mc = null;
		GetComponent<Mobile>().MoveDirection(v2, out mc);
		if (mc)
			GetComponent<MapCharacter>().Melee(mc);
	}

	void Start()
	{
		TurnTracker.Instance.Add(this);
		targetLocation = IntVector2.RoundFrom(transform.position);
	}

	void OnDestroy()
	{
		TurnTracker.Instance.Remove(this);
	}

	public class StartTurnEvent : UnityEvent<SimpleBehaviour> { }
}
