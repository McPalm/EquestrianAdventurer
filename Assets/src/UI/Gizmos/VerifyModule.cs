using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VerifyModule : MonoBehaviour
{

	public Image targetGraphic;
	public Color trueColor = new Color(0.29f, 1f, 0.32f);
	public Color falseColor = new Color(1f, 0.3f, 0.35f);
	public Color emptyColor = Color.white;

	public UnityEvent EventTrue = new UnityEvent();
	public UnityEvent EventFalse = new UnityEvent();

	public void VerifyString(string s)
	{
		int i = 0;

		if (s == "") i = 0;
		else i = (MapModule.Get(s)) ? 1 : 2;

		if(i == 0)
		{
			targetGraphic.color = emptyColor;
			EventFalse.Invoke();
		}
		if(i == 1)
		{
			targetGraphic.color = trueColor;
			EventTrue.Invoke();
		}
		if(i == 2)
		{
			targetGraphic.color = falseColor;
			EventFalse.Invoke();
		}
	}


}
