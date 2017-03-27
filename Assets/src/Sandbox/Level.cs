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
		int sum = (int)(c.baseDamage + c.baseHP/2 + c.armor + c.hitSkill + c.dodgeSkill);
		sum /= 4;
		sum *= sum;
		return sum;
	}
}
