using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	public int experience;
	public int level = 1;

	public int NextLevel
	{
		get
		{
			return level * level * 100;
		}
	}

	void Start()
	{
		GetComponent<MapCharacter>().EventKillingBlow.AddListener(OnKillingBlow);
	}

	void OnKillingBlow(MapCharacter victim)
	{
		experience += ExpWorth(victim);
		if (experience >= NextLevel)
			LevelUp();
		else
			GetComponent<MapCharacter>().EventUpdateStats.Invoke(GetComponent<MapCharacter>());
	}

	void LevelUp()
	{
		level++;
		GetComponent<MapCharacter>().SetLevel(level);
	}

	int ExpWorth(MapCharacter c)
	{
		int sum = (int)(c.Stats.damage + c.Stats.hp/2 + c.Stats.armor + c.Stats.hit + c.Stats.dodge + c.Stats.armorpen);
		sum /= 4;
		sum *= sum;
		return sum;
	}
}
