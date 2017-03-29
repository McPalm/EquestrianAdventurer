using UnityEngine;
using System.Collections;

public class GroundConsumeableFactory : GroundItem
{
	public string label;
	public int heal = 0;
	public int baseValue;

	public override Item CloneItem()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		ConsumeableFactory f = new ConsumeableFactory(label, baseValue, sr.sprite).SetColor(sr.color);

		if (heal > 0) f.SetHeal(heal);

		return f.Get();
	}


	static void Heal(GameObject target)
	{

	}
}
