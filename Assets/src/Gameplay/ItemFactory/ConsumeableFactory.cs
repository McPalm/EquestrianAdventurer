using UnityEngine;
using System.Collections;

public class ConsumeableFactory
{
	string label;
	string tooltip;
	int worth;
	int turns = 1;
	Sprite sprite;
	System.Action<GameObject> effect = null;
	Color color = Color.white;

	public ConsumeableFactory(string name, int worth, Sprite s)
	{
		label = name;
		this.worth = worth;
		sprite = s;
	}

	public Consumable Get()
	{
		Consumable c = new Consumable();

		if (turns == 0)
			tooltip += "\n using this item takes no time.";
		else if(turns > 1)
				tooltip += "\n using this item takes " + turns + " turns.";

		if (effect == null) c.Use = Nothing;
		else c.Use = effect;

		c.turns = turns;
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

	public ConsumeableFactory HealOverTime(int duration, int factor, bool idleOnly)
	{
		tooltip += "\nHeals for " + (duration * factor) + " over " + duration + " turns.";
		effect += (GameObject o) =>
		{
			HealOverTime(o, duration, factor, label, sprite, idleOnly);
		};
		return this;
	}

	public ConsumeableFactory SetStatBoost(Stats s, int duration, BaseAttributes b)
	{
		effect += (GameObject o) =>
		{
			DurationBuff(o, sprite, label, duration, s, color, b);
		};
		return this;
	}

	public ConsumeableFactory Return()
	{
		tooltip += "\nReturns home after a short delay.";
		effect += (GameObject o) =>
		{
			Return r = o.GetComponent<Return>();
			if (r) r.ReturnTimer(Random.Range(10, 20));
		};
		return this;
	}

	static public void LightHeal(GameObject o)
	{
		HitPoints hp = o.GetComponent<HitPoints>();
		hp.Heal(new DamageData(o).SetDamage(10));
	}

	static public void MediumHeal(GameObject o)
	{
		HitPoints hp = o.GetComponent<HitPoints>();
		hp.Heal(new DamageData(o).SetDamage(25));
	}

	static public void BigHeal(GameObject o)
	{
		HitPoints hp = o.GetComponent<HitPoints>();
		hp.Heal(new DamageData(o).SetDamage(75));
	}

	static public void Nothing(GameObject o)
	{
		CombatTextPool.Instance.PrintAt(o.transform.position, "But nothing happened...", Color.magenta, 1.5f);
	}

	static public void DurationBuff(GameObject o, Sprite s, string name, int duration, Stats stats, Color c, BaseAttributes b)
	{
		DurationAura a = o.AddComponent<DurationAura>();
		a.displayName = name;
		a.duration = duration;
		a.stats = stats;
		a.Icon = s;
		a.attributes = b;
	}

	static public void HealOverTime(GameObject o, int duration, int factor, string name, Sprite s, bool idleOnly)
	{
		HealOverTime a = o.AddComponent<HealOverTime>();
		a.displayName = name;
		a.duration = duration;
		a.healFactor = factor;
		a.Icon = s;
		a.idleOnly = idleOnly;
	}
}
