using UnityEngine;
using System.Collections;

public class VictoryTokenHack : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		VictoryToken v = new VictoryToken();
		v.sprite = GetComponent<SpriteRenderer>().sprite;
		GetComponent<GroundItem>().item = v;

	}
	

	private class VictoryToken : Item
	{
		public VictoryToken()
		{
			category = ItemCategory.valuable;
			displayName = "Victory Token";
			value = 1337;
			Tint = Color.white;
		}

		public override string Tooltip
		{
			get
			{
				return base.Tooltip + "\nGood job, here is a star.\nI'm honestly amazed, surpised\nand honoured you played the game\nthis far.";
			}
		}

		public override Item Clone()
		{
			VictoryToken v = new VictoryToken();
			v.sprite = sprite;
			return v;
		}
	}
}
