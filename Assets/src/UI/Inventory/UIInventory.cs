using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UIInventory : MonoBehaviour
{
	public DropArea Equipment;

	public UIEquipmentSlot WeaponSlot;
	public UIEquipmentSlot ArmorSlot;

	void Start()
	{
		// setup events


	}


	public class ItemEvent : UnityEvent<Item> { }
}
