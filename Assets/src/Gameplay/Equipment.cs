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
}
