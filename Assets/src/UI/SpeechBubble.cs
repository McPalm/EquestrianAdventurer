using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeechBubble : MyBehaviour
{
	[SerializeField]
	Text text;
	[SerializeField]
	Image Background;


	GameObject target;
	Vector2 worldLocation;

	public void Show(GameObject target, string text, float duration = 0f)
	{
		this.target = target;
		Show(text, duration);
	}

	public void Show(Vector2 worldLocation, string text, float duration = 0f)
	{
		target = null;
		this.worldLocation = worldLocation;
		Show(text, duration);
	}

	void Show(string text, float duration)
	{
		gameObject.SetActive(true);
		this.text.text = text;
		if (duration == 0f) duration = text.Length * 0.05f + 4f;
		StartCoroutine(CloseAfter(duration));
	}

	IEnumerator CloseAfter(float seconds)
	{
		yield return new WaitForSecondsRealtime(seconds);
		gameObject.SetActive(false);
	}

	void LateUpdate()
	{
		if (target)
			PutAt(target.transform.position);
		else
			PutAt(worldLocation);
	}

	void PutAt(Vector2 location)
	{
		transform.position = Camera.main.WorldToScreenPoint(location);
	}
}
