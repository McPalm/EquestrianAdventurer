using UnityEngine;
using System.Collections;

public class GroundFood : GroundItem
{
	[Space(10)]
	public Stats stats;
	public int healingRate = 20;
	public int value;
	public string displayName;


	public override Item CloneItem()
	{
		Consumable c = new Consumable();
		c.sprite = GetComponent<SpriteRenderer>().sprite;
		c.Tint = GetComponent<SpriteRenderer>().color;
		c.value = value;
		c.displayName = displayName;
		c.description = stats.NeatStringSkipEmpty(0) + "\n" + "Recover 1 HP per " + healingRate + " turns.";

		c.Use = (GameObject o) =>
		{
			Consume(o, healingRate, stats, c.sprite);
		};

		return c;
	}


	static void Consume(GameObject o, int healingrate, Stats stats, Sprite s)
	{
		FoodAura f = o.AddComponent<FoodAura>();
		f.healRate = healingrate;
		f.stats = stats;
		f.Icon = s;
		f.duration = 1600;
	}
}
