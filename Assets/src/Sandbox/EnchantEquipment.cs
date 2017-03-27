using UnityEngine;
using System.Collections;

public class EnchantEquipment : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Equipment e = GetComponent<GroundEquipment>().equipment;

		// chance to improve any equipment, making it better or worse.

		if(Random.value < 0.2f) // poor quality
		{
			e.displayName = "Poor " + e.displayName;
			if (e.slots == EquipmentType.body)
			{
				e.armor = e.armor * 3 / 4;
				e.dodge -= 2;
			}
			else
			{
				e.damage -= 1;
				e.hit -= 2;
			}
		}
		else if (Random.value < 0.2f) // fine
		{
			e.displayName = "Fine " + e.displayName;
			if (e.slots == EquipmentType.body)
			{
				e.dodge += 1;
			}
			else
			{
				e.hit += 1;
			}
		}
		else if(Random.value < 0.1f) // magical
		{
			int plus = Mathf.Min(Random.Range(1, 6), Random.Range(1, 6));

			e.displayName = e.displayName + " +" + plus;

			if (e.slots == EquipmentType.body)
			{
				e.dodge += 1;
				e.armor += plus;
			}
			else
			{
				e.hit += plus;
				e.damage += plus;
			}
		}
	}

}
