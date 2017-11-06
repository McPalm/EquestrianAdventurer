using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeechBubblePool : MonoBehaviour
{
	[SerializeField]
	SpeechBubble prefab;

	List<SpeechBubble> pool = new List<SpeechBubble>();

	static SpeechBubblePool _instance;

	public static SpeechBubblePool Instance
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType<SpeechBubblePool>();
			return _instance;
		}
	}

	public void Show(GameObject target, string text)
	{
		GetNext().Show(target, text);
	}

	public void Show(Vector2 location, string text)
	{
		GetNext().Show(location, text);
	}

	SpeechBubble GetNext()
	{
		foreach (SpeechBubble bubble in pool)
		{
			if (!bubble.gameObject.activeSelf)
			{
				return bubble;
			}
		}

		SpeechBubble bub = Instantiate(prefab);
		bub.transform.SetParent(transform);
		pool.Add(bub);
		return bub;

	}
}
