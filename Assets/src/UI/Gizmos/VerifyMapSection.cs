using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VerifyMapSection : MonoBehaviour
{

	public Image targetGraphic;
	public Color trueColor = new Color(0.29f, 1f, 0.32f);
	public Color falseColor = new Color(1f, 0.3f, 0.35f);
	public Color emptyColor = Color.white;

	public BoolEvent EventVerified = new BoolEvent();

	public void Verify(string name)
	{
		if (name == "")
		{
			targetGraphic.color = emptyColor;
			EventVerified.Invoke(false);
		}
		else
		{
			if (MapSectionData.TryGet(name) != null)
			{
				targetGraphic.color = trueColor;
				EventVerified.Invoke(true);
			}
			else
			{
				targetGraphic.color = falseColor;
				EventVerified.Invoke(false);
			}
		}
	}

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
}
