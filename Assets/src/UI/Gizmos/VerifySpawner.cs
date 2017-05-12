using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class VerifySpawner : MonoBehaviour
{

	public Image targetGraphic;
	public Color trueColor = new Color(0.29f, 1f, 0.32f);
	public Color falseColor = new Color(1f, 0.3f, 0.35f);
	public Color emptyColor = Color.white;

	public BoolEvent EventVerified = new BoolEvent();

	public void Verify(string name)
	{
		CreatureSpawner spawn = CreatureSpawner.Get(name, false);

		if (name == "")
		{
			targetGraphic.color = emptyColor;
			EventVerified.Invoke(false);
		}
		else
		{
			if (spawn != null && spawn is PremadeSpawner)
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
