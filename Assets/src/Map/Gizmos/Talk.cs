using UnityEngine;
using System.Collections;


// component to simply say something simple in a speech bubble
public class Talk : MonoBehaviour
{
	[SerializeField]
	string text;
	[SerializeField]
	GameObject anchor;

	int test = 0;

	// Update is called once per frame
	void FixedUpdate ()
	{
		test++;


		if(test == 300)
		{
			test = 0;
			if (!anchor)
			{
				anchor = new GameObject("Speech Anchor");
				anchor.transform.parent = transform;
				anchor.transform.localPosition = new Vector2(0f, 0.65f);
			}
			SpeechBubblePool.Instance.Show(anchor, text);
		}
	}
}
