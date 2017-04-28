using UnityEngine;
using UnityEngine.UI;

public class VerifyModule : MonoBehaviour
{

	public Image targetGraphic;
	public Color trueColor = new Color(0.29f, 1f, 0.32f);
	public Color falseColor = new Color(1f, 0.3f, 0.35f);
	public Color emptyColor = Color.white;

		 

	public void VerifyString(string s)
	{
		if (s == "") targetGraphic.color = emptyColor;
		else targetGraphic.color = (MapModule.Get(s)) ? trueColor : falseColor;
	}


}
