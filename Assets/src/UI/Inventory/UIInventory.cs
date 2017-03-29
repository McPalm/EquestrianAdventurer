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

	void Start()
	{
		
		model.EventAddItem.AddListener(ModelAddItem);
		model.EventEquipItem.AddListener(ModelEquip);
		model.EventDropItem.AddListener(ModelRemove);
		model.EventDestroyItem.AddListener(ModelRemove);

		Equipment.EventDropHere.AddListener(OnDragToInventory);
		Equipment.EventDropOutside.AddListener(OnDropOutside);
		Equipment.EventMoveOut.AddListener(OnDragFromInventory);

		WeaponSlot.EventDropOutside.AddListener(OnDropOutside);
		ArmorSlot.EventDropOutside.AddListener(OnDropOutside);

		Equipment.capacity = model.inventorySize + 1; // we add one, its an extra slot to allow moving items around.

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
		UIItem ui;
		if (UIItemPool.Instance.Get(i, out ui))
		{
			ui.transform.position = Camera.main.WorldToScreenPoint(model.transform.position);
		}
		ui.DropIn(Equipment);
	}

	void ModelEquip(Equipment e, EquipmentType s)
	{
		UIItem ui = UIItemPool.Instance.Get(e);
		switch(s)
		{
			case EquipmentType.body:
				//ArmorSlot.Drop(ui, null);
				ui.DropIn(ArmorSlot);
				break;
			case EquipmentType.weapon:
				ui.DropIn(WeaponSlot);
				break;
			default:
				Debug.LogWarning("Viewer unable to equip slot: " + s.ToString());
				break;
		}
	}

	void ModelRemove(Item i)
	{
		UIItemPool.Instance.Deactivate(i);
	}


	///
	/// Events incomming from the view
	///
	
	void OnDragFromInventory(Draggable d, DropArea source, DropArea destination)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if(ui.Item is Equipment && (destination == WeaponSlot || destination == ArmorSlot) )
		{
			model.EquipItem(ui.Item as Equipment);
		}
		else if (source == Equipment && destination == Equipment)
		{
			model.Consume(ui.Item);
		}
	}

	void OnDragToInventory(Draggable d, DropArea source, DropArea destination)
	{
		if(source == WeaponSlot)
			model.UnEquip(EquipmentType.weapon);
		else if(source == ArmorSlot)
			model.UnEquip(EquipmentType.body);
	}

	void OnDropOutside(Draggable d, DropArea a)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if (a == Equipment)
			model.DropItem(ui.Item);
		else if (a == WeaponSlot)
			model.UnEquip(EquipmentType.weapon, true);
		else if (a == ArmorSlot)
			model.UnEquip(EquipmentType.body, true);
	}

	public class ItemEvent : UnityEvent<Item> { }
}
