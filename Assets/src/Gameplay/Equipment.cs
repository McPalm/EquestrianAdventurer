using UnityEngine;
using System.Collections;

[System.Serializable]
public class Equipment : Item
{
	public EquipmentType slots;

	public int armor;
	public int dodge;
	public int damage;
	public int hit;
	public int hp;

	public override string Tooltip
	{
		get
		{
			string s = displayName;
			if (armor > 0) s += "\n Armor: " + armor;
			if (damage > 0) s += "\n Damage: " + damage;
			if (dodge > 0) s += "\n Dodge: " + dodge;
			if (hp > 0) s += "\n Health: " + hp;
			if (hit > 0) s += "\n Hit: " + hit;
			return s;
		}
	}
}
