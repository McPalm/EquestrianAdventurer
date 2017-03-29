using UnityEngine;
using System.Collections;

public class ConsumeableFactory
{
	string label;
	string tooltip;
	int worth;
	Sprite sprite;
	System.Action<GameObject> effect = null;
	Color color = Color.white;

	public ConsumeableFactory(string name, int worth, Sprite s)
	{
		label = name;
		this.worth = worth;
		sprite = s;
	}

	public Consumeable Get()
	{
		Consumeable c = new Consumeable();

		if (effect == null) c.Use = Nothing;
		else c.Use = effect;

		c.value = worth;
		c.displayName = label;
		c.description = tooltip;
		c.sprite = sprite;
		c.red = color.r;
		c.green = color.g;
		c.blue = color.b;
		return c;
	}

	public ConsumeableFactory SetColor(Color c)
	{
		color = c;
		return this;
	}

	public ConsumeableFactory SetHeal(int i)
	{
		if (i == 1)
		{
			tooltip += "\nHeals 10 hitpoints.";
			worth *= 3;
			worth /= 2;
			worth += 10;
			effect += LightHeal;	
		}
		else if(i == 2)
		{
			tooltip += "\nHeals 25 hitpoints.";
			worth *= 3;
			worth /= 2;
			worth += 50;
			effect += MediumHeal;
		}
		else
		{
			tooltip += "\nHeals 75 hitpoints.";
			worth *= 3;
			worth /= 2;
			worth += 200;
			effect += BigHeal;
		}
		return this;
	}

	static public void LightHeal(GameObject o)
	{
		HitPoints hp = o.GetComponent<HitPoints>();
		hp.Heal(new DamageData().SetDamage(10));
	}

	static public void MediumHeal(GameObject o)
	{
		HitPoints hp = o.GetComponent<HitPoints>();
		hp.Heal(new DamageData().SetDamage(25));
	}

	static public void BigHeal(GameObject o)
	{
		HitPoints hp = o.GetComponent<HitPoints>();
		hp.Heal(new DamageData().SetDamage(75));
	}

	static public void Nothing(GameObject o)
	{
		CombatTextPool.Instance.PrintAt(o.transform.position, "But nothing happened...", Color.magenta, 1.5f);
	}
}
