using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour
{
	// Vector2 NetworkedPosition = new Vector2(999999999f, 99999999999f);

	public void MoveTo(Vector2 v2)
	{
		//NetworkedPosition = new Vector2(Mathf.Round(v2.x), Mathf.Round(v2.y));
		OnMove(v2);
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

