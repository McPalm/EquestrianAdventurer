using UnityEngine;
using System.Collections;


// component to simply say something simple in a speech bubble
public class Talk : MonoBehaviour
{
	[SerializeField]
	string text;
	[SerializeField]
	GameObject anchor;

	public void SaySomething()
	{
		Say(text);
	}


	public void Say(string text)
	{
		if (!anchor)
		{
			anchor = new GameObject("Speech Anchor");
			anchor.transform.parent = transform;
			anchor.transform.localPosition = new Vector2(0f, 0.7f);
		}
		SpeechBubblePool.Instance.Show(anchor, text);
	}
}
