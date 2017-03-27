using UnityEngine;
using System.Collections;



public class ToggleLocations : MonoBehaviour
{

	public Transform ShowAnchor;
	public Transform HideAnchor;

	public Transform target;

	public void Hide()
	{
		StopAllCoroutines();
		StartCoroutine(SlideTo(target, HideAnchor.position));
	}

	public void Show()
	{
		StopAllCoroutines();
		StartCoroutine(SlideTo(target, ShowAnchor.position));
	}

	public void Toggle()
	{
		if (OnScreen) Hide();
		else Show();
	}

	bool OnScreen
	{
		get
		{
			return target.position.x > 50f && target.position.y > 50f && target.position.x < Screen.width - 50f && target.position.y < Screen.height;
		}
	}

	IEnumerator SlideTo(Transform client, Vector3 destination)
	{
		Vector3 start = client.position;

		for(float timer = 0; timer < 1f; timer += Time.deltaTime * 6f)
		{
			client.position = Vector3.Lerp(start, destination, timer);
			yield return new WaitForSeconds(0f);
		}

		client.position = destination;
	}

}
