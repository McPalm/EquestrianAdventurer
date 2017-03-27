using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
	public Inventory model; // the players inventory class

	[Space(10)] // view component
	public DropArea Equipment;
	public UIEquipmentSlot WeaponSlot;
	public UIEquipmentSlot ArmorSlot;

	[Space(10)]
	public UIItem UIItemPrefab;

	List<UIItem> active = new List<UIItem>();

	void Start()
	{
		model.EventAddItem.AddListener(ModelAddItem);
		model.EventEquipItem.AddListener(ModelEquip);
		model.EventDropItem.AddListener(ModelDrop);

		WeaponSlot.EventAdd.AddListener(ViewEquip);
		ArmorSlot.EventAdd.AddListener(ViewEquip);

		WeaponSlot.EventRemove.AddListener(ViewUnEquip);
		ArmorSlot.EventRemove.AddListener(ViewUnEquip);
	}

	/// <summary>
	/// Whenever the Inventory gets a new item added.
	/// Such as picking up from the ground
	/// </summary>
	/// <param name="i"></param>
	void ModelAddItem(Item i)
	{
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
		foreach (UIItem ui in active)
		{
			if(ui.Item == i)
			{
				active.Remove(ui);
				Destroy(ui.gameObject); // we should pool
				return;
			}
		}
	}


	/// View Events, stuffs that is done in the UI that should update the players inventory
	
	void ViewDropEvent(Draggable d) // when we drop something outside the Inventory
	{
		UIItem i = d as UIItem;
		if (Equipment.Contains(d as Dropable))
		{
			model.DropItem(i.Item);
		}
		else if (i.Item is Equipment)
		{
			model.UnEquip(i.Item as Equipment, true);
		}
		else
		{
			Debug.LogWarning("Unhandled Drop Event");
		}
	}

	void ViewEquip(Draggable d)
	{
		UIItem i = d as UIItem;
		model.EquipItem((Equipment)i.Item);
	}

	void ViewUnEquip(Draggable d)
	{
		UIItem i = d as UIItem;
		model.UnEquip((Equipment)i.Item);
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
		ret.transform.position = Camera.main.WorldToScreenPoint(model.transform.position); // make the item over the player. Should add a cool effect of it going from the ground to the inventory
		ret.EventDropOutside.AddListener(ViewDropEvent);
		return ret;
	}

	public class ItemEvent : UnityEvent<Item> { }
}
