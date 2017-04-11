using UnityEngine;
using UnityEngine.UI;
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
	public DropArea TrinketSlot;
	public DropArea HoovesSlot;
	public DropArea HeadSlot;

	[Space(10)] // view component

	public DropArea[] Consumables;
	public Text[] StackCounters;

	[Space(10)] // view component

	public DropArea DialogueDrop;

	public ItemEvent EventSellItem = new ItemEvent();
		 

	void Start()
	{
		
		model.EventAddItem.AddListener(ModelAddItem);
		model.EventEquipItem.AddListener(ModelEquip);
		model.EventDropItem.AddListener(ModelRemove);
		model.EventDestroyItem.AddListener(ModelRemove);
		model.EventRemoveConsumable.AddListener(ModelRemove);

		Equipment.EventDropHere.AddListener(OnDragToInventory);
		Equipment.EventDropOutside.AddListener(OnDropOutside);
		Equipment.EventMoveOut.AddListener(OnDragFromInventory);
		Equipment.EventClick.AddListener(OnClickInventory);

		WeaponSlot.EventDropOutside.AddListener(OnDropOutside);
		ArmorSlot.EventDropOutside.AddListener(OnDropOutside);
		TrinketSlot.EventDropOutside.AddListener(OnDropOutside);
		HoovesSlot.EventDropOutside.AddListener(OnDropOutside);
		HeadSlot.EventDropOutside.AddListener(OnDropOutside);

		model.EventAddConsumable.AddListener(ModelAddConsumeable);
		for (int i = 0; i < Consumables.Length; i++)
		{
			Consumables[i].EventDropOutside.AddListener(OnDropOutside);
			Consumables[i].EventDropHere.AddListener(OnDragToConsumableBar);
			Consumables[i].EventMoveOut.AddListener(OnDragFromInventory);
			Consumables[i].EventClick.AddListener(OnClickConsumeable);
		}

		Equipment.capacity = model.inventorySize + 1; // we add one, its an extra slot to allow moving items around.

		/*
		WeaponSlot.EventAdd.AddListener(ViewEquip);
		ArmorSlot.EventAdd.AddListener(ViewEquip);

		WeaponSlot.EventRemove.AddListener(ViewUnEquip);
		ArmorSlot.EventRemove.AddListener(ViewUnEquip);
		*/

		foreach(Item i in model.EquipmentEnumerator)
		{
			ModelAddItem(i);
		}
		RefreshStacks();
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
			case EquipmentType.trinket:
				ui.DropIn(TrinketSlot);
				break;
			case EquipmentType.hooves:
				ui.DropIn(HoovesSlot);
				break;
			case EquipmentType.head:
				ui.DropIn(HeadSlot);
				break;
			default:
				Debug.LogWarning("Viewer unable to equip slot: " + s.ToString());
				break;
		}
	}

	void ModelRemove(Item i)
	{
		UIItemPool.Instance.Deactivate(i);
		RefreshStacks();
	}

	void ModelAddConsumeable(Item c, int slot, int stack)
	{
		if (stack > 0)
			UIItemPool.Instance.Deactivate(c);
		else
		{
			UIItem ui = UIItemPool.Instance.Get(c);
		
			if (slot < Consumables.Length)
				ui.DropIn(Consumables[slot]);
			else
				Debug.LogError("Slot " + slot + " not available in inventory");
		}
		RefreshStacks();
	}


	void RefreshStacks()
	{
		for (int i = 0; i < Consumables.Length; i++)
		{
			int s = model.StacksInSlot(i);
			if (s < 2)
				StackCounters[i].text = "";
			else
				StackCounters[i].text = "x" + s;
		}
	}

	///
	/// Events incomming from the view
	///

	void OnClickInventory(Draggable d)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
		{
			EventSellItem.Invoke(ui.Item);
		}
		else if (ui.Item is Equipment)
		{
			model.EquipItem(ui.Item as Equipment);
		}
	}
	
	void OnDragFromInventory(Draggable d, DropArea source, DropArea destination)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if(ui.Item is Equipment && (destination == WeaponSlot || destination == ArmorSlot || destination == TrinketSlot || destination == HoovesSlot || destination == HeadSlot) )
		{
			model.EquipItem(ui.Item as Equipment);
		}
		else if(destination == DialogueDrop)
		{
			model.Gift(FindObjectOfType<UIDialogueWindow>().client, ui.Item);
		}
	}

	void OnDragToInventory(Draggable d, DropArea source, DropArea destination)
	{
		if(source == WeaponSlot)
			model.UnEquip(EquipmentType.weapon);
		if(source == ArmorSlot)
			model.UnEquip(EquipmentType.body);
		if (source == TrinketSlot)
			model.UnEquip(EquipmentType.trinket);
		if (source == HeadSlot)
			model.UnEquip(EquipmentType.head);
		else if (source == HoovesSlot)
			model.UnEquip(EquipmentType.hooves);
	}

	void OnDropOutside(Draggable d, DropArea a)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if (a == Equipment)
		{
			if (!TryGiftUnderMouse(ui))
				model.DropItem(ui.Item);
		}
		else if (a == WeaponSlot)
			model.UnEquip(EquipmentType.weapon, true);
		else if (a == ArmorSlot)
			model.UnEquip(EquipmentType.body, true);
		else if (a == TrinketSlot)
			model.UnEquip(EquipmentType.trinket, true);
		else if (a == HoovesSlot)
			model.UnEquip(EquipmentType.hooves, true);
		else if (a == HeadSlot)
			model.UnEquip(EquipmentType.head, true);
		else
		{
			TryGiftUnderMouse(ui);
			model.DropItem(ui.Item); // in theory, this should let us drop items from the consumable bar.
		}
	}

	bool TryGiftUnderMouse(UIItem ui)
	{
		MapCharacter c = ObjectMap.Instance.CharacterAt(IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
		if(c)
		{
			model.Gift(c.gameObject, ui.Item);
			return true;
		}
		return false;
	}

	void OnDragToConsumableBar(Draggable d, DropArea source, DropArea destination)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;

		/*
		if(source == destination)
		{
			model.Consume(ui.Item);
			return;
		}
		*/
		
		for (int i = 0; i < Consumables.Length; i++)
		{
			if (destination == Consumables[i])
			{
				model.MoveConsumable(ui.Item, i);
			}
		}
		
	}

	void OnClickConsumeable(Dropable d)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
			EventSellItem.Invoke(ui.Item);
		else
			model.Consume(ui.Item);
	}

	public class ItemEvent : UnityEvent<Item> { }
}
