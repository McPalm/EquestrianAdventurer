using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Mobile : MapObject
{
	/// <summary>
	/// Event called whenever we move
	/// param 1 is desination
	/// param 2 is vector of movement
	/// </summary>
	public MovementEvent EventMovement = new MovementEvent();

	// Vector2 NetworkedPosition = new Vector2(999999999f, 99999999999f);

		/// <summary>
		/// Move the character in the given direction
		/// </summary>
		/// <param name="v2">direction of movement</param>
		/// <param name="bump">out parameter for a character that blocks movement, nullable</param>
		/// <returns>true if we moved, false if we could not move</returns>
	public bool MoveDirection(Vector2 v2, out MapCharacter bump, out Interactable iBump)
	{
		// destination of move
		IntVector2 d = RealLocation + IntVector2.RoundFrom(v2);
		bump = null;

		// figure out if we can enter
		if (BlockMap.Instance.BlockMove(d, out iBump)) return false;

		// check for other characters

		bump = ObjectMap.Instance.CharacterAt(d);
		if (bump)
		{
			iBump = bump.GetComponent<Interactable>();
			return false;
		}
		
		ForceMove(d);
		EventMovement.Invoke((Vector2)d, v2);

		return true;
	}

	public void ForceMove(IntVector2 v2)
	{
		//NetworkedPosition = new Vector2(Mathf.Round(v2.x), Mathf.Round(v2.y));
		OnMove(v2);
		RealLocation = v2;
	}

	public void ForceMove(IntVector2 v2, float duration)
	{
		StopAllCoroutines();
		StartCoroutine(Tween(transform.position, (Vector2)v2, duration));
		RealLocation = v2;
	}

	void OnMove(IntVector2 v2)
	{
		StopAllCoroutines();
		float distance = ((Vector2)transform.position - (Vector2)v2).magnitude;
		StartCoroutine(Tween(transform.position, (Vector2)v2, distance/10f + 0.1f));
	}

	public void Stop()
	{
		StopAllCoroutines();
	}

	IEnumerator Tween(Vector2 start, Vector2 destination, float duration)
	{
		if (duration > 0.4f) duration = 0.4f; // prevent slow moves
		float time = 0f;
		while(time < duration)
		{
			time += Time.deltaTime;
			transform.position = Vector2.Lerp(start, destination, time / duration);
			yield return new WaitForSeconds(0f);
		}
		transform.position = destination;
	}

	public class MovementEvent : UnityEvent<Vector2, Vector2> { };
}

