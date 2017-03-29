using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
	public Inventory model; // the players inventory class

	[Space(10)] // view component
	public DropArea Equipment;
	public DropArea WeaponSlot;
	public DropArea ArmorSlot;

	[Space(10)]
	public UIItem UIItemPrefab;

	List<UIItem> active = new List<UIItem>();

	void Start()
	{
		model.EventAddItem.AddListener(ModelAddItem);
		model.EventEquipItem.AddListener(ModelEquip);
		model.EventDropItem.AddListener(ModelDrop);

		/*
		WeaponSlot.EventAdd.AddListener(ViewEquip);
		ArmorSlot.EventAdd.AddListener(ViewEquip);

		WeaponSlot.EventRemove.AddListener(ViewUnEquip);
		ArmorSlot.EventRemove.AddListener(ViewUnEquip);
		*/
	}

	/// <summary>
	/// Whenever the Inventory gets a new item added.
	/// Such as picking up from the ground
	/// </summary>
	/// <param name="i"></param>
	void ModelAddItem(Item i)
	{
		Equipment.Drop(UIItemPool.Instance.Get(i), null);
	}

	void ModelEquip(Equipment e, EquipmentType s)
	{
		UIItem ui = UIItemPool.Instance.Get(e);
		switch(s)
		{
			case EquipmentType.body:
				ArmorSlot.Drop(ui, null);
				break;
			case EquipmentType.weapon:
				WeaponSlot.Drop(ui, null);
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
	

	public class ItemEvent : UnityEvent<Item> { }
}
