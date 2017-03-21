using UnityEngine;
using System.Collections;

public class Mobile : MapObject
{
	// Vector2 NetworkedPosition = new Vector2(999999999f, 99999999999f);

	public bool MoveDirection(Vector2 v2, out MapCharacter bump)
	{
		// destination of move
		Vector2 d = (Vector2)RealLocation + v2;
		bump = null;

		// figure out if we can enter
		if (BlockMap.Instance.BlockMove(d)) return false;

		// check for other characters
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, v2, 1f, 1 << 8);

		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider.gameObject != gameObject)
			{
				bump = hit.collider.GetComponent<MapCharacter>();
				return false;
			}
		}

		// perform the movement
		ForceMove(d);

		return true;
	}

	public void ForceMove(Vector2 v2)
	{
		//NetworkedPosition = new Vector2(Mathf.Round(v2.x), Mathf.Round(v2.y));
		OnMove(v2);
		RealLocation = IntVector2.RoundFrom(v2);
	}

	void OnMove(Vector2 v2)
	{
		StopAllCoroutines();
		float distance = ((Vector2)transform.position - v2).magnitude;
		StartCoroutine(Tween(transform.position, v2, distance/10f + 0.1f));
		// transform.position = v2;
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
}

