using UnityEngine;
using System.Collections;

public class FoodAura : DurationAura
{
	public int healRate;

	int tick;

	public override string Tooltip
	{
		get
		{
			return base.Tooltip +
				"\nHeals 1 hp every " + healRate + " turns." +
				"\nCan only benefit from one food item at a time.";
		}
	}

	// There can be only one!
	new protected void OnEnable()
	{
		foreach(FoodAura f in GetComponents<FoodAura>())
		{
			if (f != this)
				Destroy(f);
		}
		CharacterActionController ac = GetComponent<CharacterActionController>();
		if (ac)
		{
			ac.EventAfterAction.AddListener(OnEndTurn);
		}
		base.OnEnable();
		GetComponent<MapCharacter>().Refresh();
	}

	void OnEndTurn(CharacterActionController cac, CharacterActionController.Actions a)
	{
		tick++;
		tick %= healRate;
		if (tick == 0) GetComponent<HitPoints>().Heal(new DamageData(gameObject).SetDamage(1));
	}
}
