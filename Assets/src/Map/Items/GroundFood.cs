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
		float modifier = Random.Range(0.75f, 1.25f);

		if(Random.value < 0.2f)
		{
			c.value = (int)(value * 10 * modifier);
			c.displayName = "Gourmet " + displayName;
			c.Use = (GameObject o) =>
			{
				Consume(o, (int)(healingRate * 0.75f / modifier), stats + stats, c.sprite);
			};
			c.description = (stats + stats).NeatStringSkipEmpty(0) + "\n" + "Recover 1 HP per " + ((int)(healingRate * 0.75f / modifier)) + " turns.";

		}
		else
		{
			c.value = (int)(modifier * value);
			c.displayName = displayName;
			c.Use = (GameObject o) =>
			{
				Consume(o, (int)(healingRate * 1f / modifier), stats, c.sprite);
			};
			c.description = stats.NeatStringSkipEmpty(0) + "\n" + "Recover 1 HP per " + (int)(healingRate * 1f / modifier) + " turns.";
		}

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
