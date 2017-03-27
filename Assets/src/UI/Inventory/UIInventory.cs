using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
	public Inventory target; // the model

	[Space(10)] // view component
	public DropArea Equipment;
	public UIEquipmentSlot WeaponSlot;
	public UIEquipmentSlot ArmorSlot;

	[Space(10)]
	public UIItem UIItemPrefab;

	List<UIItem> active = new List<UIItem>();

	void Start()
	{
		target.EventAddItem.AddListener(ModelAddItem);
		target.EventEquipItem.AddListener(ModelEquip);
		target.EventDropItem.AddListener(ModelDrop);
	}

	/// <summary>
	/// Whenever the Inventory gets a new item added.
	/// Such as picking up from the ground
	/// </summary>
	/// <param name="i"></param>
	void ModelAddItem(Item i)
	{
		print("Adding "+ i.displayName);
		Equipment.Drop(Get(i));
	}

	void ModelEquip(Equipment e, EquipmentType s)
	{
		UIItem ui = Get(e);
		switch(s)
		{
			case EquipmentType.body:
				ArmorSlot.Drop(ui);
				break;
			case EquipmentType.weapon:
				WeaponSlot.Drop(ui);
				break;
			default:
				Debug.LogWarning("Viewer unable to equip slot: " + s.ToString());
				break;
		}
	}

	void ModelDrop(Item i)
	{
		foreach(UIItem ui in active)
		{
			if(ui.Item == i)
			{
				active.Remove(ui);
				Destroy(ui.gameObject); // we should pool
				return;
			}
		}
	}

	/// <summary>
	/// Get the UI item for the item.
	/// Makes a new one if none exsist already
	/// </summary>
	/// <param name="i"></param>
	/// <returns></returns>
	UIItem Get(Item i)
	{
		foreach(UIItem ui in active)
		{
			if (ui.Item == i) return ui;
		}

		return Build(i);
	}

	UIItem Build(Item i)
	{
		UIItem ret = Instantiate(UIItemPrefab);
		active.Add(ret);
		ret.transform.SetParent(transform);
		ret.Item = i;
		ret.transform.position = Camera.main.WorldToScreenPoint(target.transform.position); // make the item over the player. Should add a cool effect of it going from the ground to the inventory
		return ret;
	}

	public class ItemEvent : UnityEvent<Item> { }
}
