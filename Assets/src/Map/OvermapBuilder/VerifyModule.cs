using UnityEngine;
using UnityEngine.UI;

public class VerifyModule : MonoBehaviour
{

	public Image targetGraphic;
	public Color trueColor;
	public Color falseColor;

	public void VerifyString(string s)
	{
		targetGraphic.color = (MapModule.Get(s)) ? trueColor : falseColor;
	}


}
