﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

// an inventory for characters, equipment and other good stuffs
public class Inventory : MonoBehaviour
{
	public int inventorySize = 12;
	// public int consumableSize = 8;

	public InventoryEvent EventChangeEquipment = new InventoryEvent();
	public ItemEvent EventDropItem = new ItemEvent();
	public ItemEvent EventDestroyItem = new ItemEvent(); // Sssentially identical to drop. Tho this one is more definitive?
	public ItemEvent EventAddItem = new ItemEvent();
	public EquipEvent EventEquipItem = new EquipEvent();
	public EquipEvent EventUnEquipItem = new EquipEvent();
	public ItemEvent EventRemoveConsumable = new ItemEvent();
	public ConsumableEvent EventAddConsumable = new ConsumableEvent();

	List<Item> items = new List<Item>(6);

	const int SLOTS = 8;
	const int STACKS = 4;
	Consumable[,] consumables = new Consumable[SLOTS,STACKS];

	Equipment bodySlot;
	Equipment weaponSlot;
	Equipment trinketSlot;
	Equipment hoovesSlot;
	Equipment headSlot;

	private bool EmptySpace
	{
		get
		{
			return inventorySize - items.Count > 0;
		}
	}

	public bool debugprint;
	void Update()
	{
		if (debugprint) PrintInventory();
		debugprint = false;
	}

	public bool CanAccept(Item i)
	{
		if (i.category == ItemCategory.gold) return true;
		if(i is Consumable)
		{
			for (int c = 0; c < SLOTS; c++)
				if (consumables[c, 0] == null) return true;
			return false;
		}
		return EmptySpace;
	}

	public bool Contains(Item item)
	{
		if (items.Contains(item)) return true;
		for (int i = 0; i < SLOTS; i++)
			if (consumables[i, 0] == item) return true;
		return false;
	}

	public bool Gift(GameObject target, Item gift)
	{
		if (target == gameObject) return false;
		if (Contains(gift))
		{
			StoryTriggerComponent c = target.GetComponent<StoryTriggerComponent>();
			if(c && c.Gift(gift))
			{
				DestroyItem(gift);
				RemoveItem(gift);
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Picking the first item on the ground
	/// </summary>
	/// <returns>true if there is an item and it can be picked up</returns>
	public bool PickupFromGround()
	{
		GroundItem g = null;
		foreach(MapObject o in ObjectMap.Instance.ObjectsAtLocation(GetComponent<MapObject>().RealLocation))
		{
			g = o.GetComponent<GroundItem>();
			if (g) break;
		}
		if(g && TryAddItem(g.item))
		{
			Destroy(g.gameObject); // Might want to object pool this.
			return true;
		}
		return false;
	}

	/// <summary>
	/// Add an item to the inventory
	/// </summary>
	/// <param name="i">Item to put in inventory</param>
	/// <returns>true of the change went through</returns>
	public bool TryAddItem(Item i, int slot = -1)
	{
		if(i.category == ItemCategory.gold)
		{
			GetComponent<Purse>().AddBits(i.value);
			return true;
		}
		if (i is Consumable)
			return AddConsumable(i as Consumable, slot);
		if (!EmptySpace) return false;
		items.Add(i);
		EventAddItem.Invoke(i);
		return true;
	}

	public void AddOrPutOnGround(Item i, int slot = -1)
	{
		if(TryAddItem(i, slot) == false)
		{
			PutOnGround(i);
		}
	}

	void PutOnGround(Item i)
	{
		// make the item and put on ze gorund
		GameObject o = new GameObject(i.displayName);
		o.transform.position = (Vector3)GetComponent<MapObject>().RealLocation;
		o.AddComponent<GroundItem>().item = i;
		EventDropItem.Invoke(i);
	}

	public bool AddConsumable(Consumable c, int slot = -1)
	{
		if(slot >= 0 && slot < SLOTS)
		{
			if (StackInSlot(c, slot))
				return true;
		}
		for (int i = 0; i < SLOTS; i++) // try stack pass
		{
			if (consumables[i, 0] != null && StackInSlot(c, i))
			{
				return true;
			}
		}
		for (int i = 0; i < SLOTS; i++) // put in empty pass
		{
			if (StackInSlot(c, i))
			{
				return true;
			}
		}	
		return false;
	}

	public int StacksInSlot(int slot)
	{
		for(int i = 0; i < STACKS; i++)
		{
			if (consumables[slot, i] == null) return i;
		}
		return 4;
	}

	bool StackInSlot(Consumable c, int slot)
	{
		if(consumables[slot, 0] == null || consumables[slot, 0].StacksWith(c))
		{
			for(int i = 0; i < STACKS; i++)
			{
				if(consumables[slot, i] == null)
				{
					consumables[slot, i] = c;
					EventAddConsumable.Invoke(c, slot, i);
					return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// call after we remove an item from a consumable slot. Will shift all items in their right place
	/// </summary>
	/// <param name="slot"></param>
	void ShiftSlot(int slot)
	{
		for(int i = 0; i < 3; i++)
		{
			for (int off = 1; off < STACKS - i; off++)
			{
				if (consumables[slot, i] == null)
				{
					if (consumables[slot, i + off] != null)
					{
						consumables[slot, i] = consumables[slot, i + off];
						consumables[slot, i + off] = null;
						EventAddConsumable.Invoke(consumables[slot, i], slot, i);
						break;
					}
				}
			}
		}
	}

	public bool MoveConsumable(Item item, int slot)
	{
		if(item is Consumable)
		{
			for (int oSlot = 0; oSlot < SLOTS; oSlot++)
			{
				if (item == consumables[oSlot, 0])
				{
					if (consumables[slot, 0] == null)
					{
						for (int stack = 0; stack < STACKS; stack++)
						{
							consumables[slot, stack] = consumables[oSlot, stack];
							EventAddConsumable.Invoke(consumables[slot, stack], slot, stack);
							consumables[oSlot, stack] = null;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	/// <summary>
	/// Remove an item from the inventory
	/// </summary>
	/// <param name="i">the item to remove</param>
	/// <returns>true if the item is in the inventory</returns>
	public bool RemoveItem(Item i)
	{
		if(i is Consumable)
		{
			return RemoveConsumable(i as Consumable);
		}
		else if(items.Remove(i))
		{
			return true;
		}
		return false;
	}

	public bool RemoveConsumable(Consumable c)
	{
		for(int i = 0; i < SLOTS; i++)
		{
			for (int stack = 0; stack < STACKS; stack++)
			{
				if (consumables[i, stack] == c)
				{
					consumables[i, stack] = null;
					EventRemoveConsumable.Invoke(c);
					ShiftSlot(i);
					return true;
				}
			}
		}
		return false;
	}


	/// <summary>
	/// Consumes the given item if its in the inventory
	/// Adds item action to the characters action stack
	/// Destroys the item
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool Consume(Item item)
	{
		if (item is Consumable)
		{
			if (RemoveConsumable(item as Consumable))
			{
				int turns = (item as Consumable).turns;
				(item as Consumable).Use(gameObject);
				UseAction(turns);
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Destroys and item utterly.
	/// Its private, use whatever function is similar to what you wish to do instead
	/// </summary>
	/// <param name="i"></param>
	/// <returns></returns>
	bool DestroyItem(Item i)
	{
		if (items.Remove(i))
		{
			EventDestroyItem.Invoke(i);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Drop the given item to the ground
	/// </summary>
	/// <param name="i">the item in question</param>
	/// <returns>true if we could drop it</returns>
	public bool DropItem(Item i)
	{
		if(RemoveItem(i))
		{
			PutOnGround(i);
			UseAction(1);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Equip an item into an appropiate slot
	/// trying to equip into an already used slot fails automaticall
	/// </summary>
	/// <param name="e"></param>
	/// <returns>true if equipped succesfully</returns>
	public bool EquipItem(Equipment e)
	{
		switch(e.slots)
		{
			case EquipmentType.body:

				if (bodySlot != null)
				{
					inventorySize++; // I think this is the first real hack I put into this game.
					if (!UnEquip(bodySlot))
					{
						inventorySize--;
						return false;
					}
					inventorySize--;
				}
				RemoveItem(e);
				bodySlot = e;
				EventChangeEquipment.Invoke(this);
				EventEquipItem.Invoke(e, EquipmentType.body);
				UseAction(5);
				return true;
			case EquipmentType.weapon:

				if (weaponSlot != null)
				{
					inventorySize++;
					if (!UnEquip(weaponSlot))
					{
						inventorySize--;
						return false;
					}
					inventorySize--;
				}
				RemoveItem(e);
				weaponSlot = e;
				EventChangeEquipment.Invoke(this);
				EventEquipItem.Invoke(e, EquipmentType.weapon);
				UseAction(1);
				return true;
			case EquipmentType.trinket:

				if (trinketSlot != null)
				{
					inventorySize++;
					if (!UnEquip(trinketSlot))
					{
						inventorySize--;
						return false;
					}
					inventorySize--;
				}
				RemoveItem(e);
				trinketSlot = e;
				EventChangeEquipment.Invoke(this);
				EventEquipItem.Invoke(e, EquipmentType.trinket);
				UseAction(2);
				return true;
			case EquipmentType.hooves:

				if (hoovesSlot != null)
				{
					inventorySize++;
					if (!UnEquip(hoovesSlot))
					{
						inventorySize--;
						return false;
					}
					inventorySize--;
				}
				RemoveItem(e);
				hoovesSlot = e;
				EventChangeEquipment.Invoke(this);
				EventEquipItem.Invoke(e, EquipmentType.hooves);
				UseAction(2);
				return true;
			case EquipmentType.head:

				if (headSlot != null)
				{
					inventorySize++;
					if (!UnEquip(headSlot))
					{
						inventorySize--;
						return false;
					}
					inventorySize--;
				}
				RemoveItem(e);
				headSlot = e;
				EventChangeEquipment.Invoke(this);
				EventEquipItem.Invoke(e, EquipmentType.head);
				UseAction(2);
				return true;
		}
		return false;
	}


	/// <summary>
	/// Remove the item in the given slot
	/// </summary>
	/// <param name="slot">The slot wich an item will be removed from</param>
	/// <param name="drop">if true, put on the ground, if false, put in inventory</param>
	/// <returns>true if we succesfully removed the item from the slot</returns>
	public bool UnEquip(EquipmentType slot, bool drop = false)
	{
		if (!drop && !EmptySpace) return false;
		switch (slot)
		{
			case EquipmentType.body:
				if (bodySlot == null) return false;
				return UnEquip(bodySlot, drop);
			case EquipmentType.weapon:
				if (weaponSlot == null) return false;
				return UnEquip(weaponSlot, drop);
			case EquipmentType.trinket:
				if (trinketSlot == null) return false;
				return UnEquip(trinketSlot, drop);
			case EquipmentType.hooves:
				if (hoovesSlot == null) return false;
				return UnEquip(hoovesSlot, drop);
			case EquipmentType.head:
				if (headSlot == null) return false;
				return UnEquip(headSlot, drop);
		}
		return false;
	}

	/// <summary>
	/// Remove the given item from the slot that it is in
	/// </summary>
	/// <param name="item">the item to remove</param>
	/// <param name="drop">if true, put on the ground, if false, put in inventory</param>
	/// <returns>true if the item could be removed from the inventory</returns>
	public bool UnEquip(Equipment item, bool drop = false)
	{
		if (!drop && !EmptySpace) return false;
		switch (item.slots)
		{
			case EquipmentType.body:
				if (bodySlot != item) return false;
				AddOrPutOnGround(bodySlot);
				bodySlot = null;
				EventUnEquipItem.Invoke(item, EquipmentType.body);
				EventChangeEquipment.Invoke(this);
				if (drop) DropItem(item);
				return true;
			case EquipmentType.weapon:
				if (weaponSlot != item) return false;
				AddOrPutOnGround(weaponSlot);
				weaponSlot = null;
				EventUnEquipItem.Invoke(item, EquipmentType.weapon);
				EventChangeEquipment.Invoke(this);
				if (drop) DropItem(item);
				return true;
			case EquipmentType.trinket:
				if (trinketSlot != item) return false;
				AddOrPutOnGround(trinketSlot);
				trinketSlot = null;
				EventUnEquipItem.Invoke(item, EquipmentType.trinket);
				EventChangeEquipment.Invoke(this);
				if (drop) DropItem(item);
				return true;
			case EquipmentType.hooves:
				if (hoovesSlot != item) return false;
				AddOrPutOnGround(hoovesSlot);
				hoovesSlot = null;
				EventUnEquipItem.Invoke(item, EquipmentType.hooves);
				EventChangeEquipment.Invoke(this);
				if (drop) DropItem(item);
				return true;
			case EquipmentType.head:
				if (headSlot != item) return false;
				AddOrPutOnGround(headSlot);
				headSlot = null;
				EventUnEquipItem.Invoke(item, EquipmentType.head);
				EventChangeEquipment.Invoke(this);
				if (drop) DropItem(item);
				return true;
		}
		return false;
	}

	public Equipment[] GetEquipped()
	{
		List<Equipment> ret = new List<Equipment>();
		if (weaponSlot != null) ret.Add(weaponSlot);
		if (bodySlot != null) ret.Add(bodySlot);
		if (trinketSlot != null) ret.Add(trinketSlot);
		if (hoovesSlot != null) ret.Add(hoovesSlot);
		if (headSlot != null) ret.Add(headSlot);

		return ret.ToArray();
	}

	public void PrintInventory()
	{
		string printme = "<< " + name + " Inventory >>";
		printme += "\nWeapon: " + ((weaponSlot != null) ? weaponSlot.displayName : "");
		printme += "\nArmor: " + ((bodySlot != null) ? bodySlot.displayName : "");
		printme += "\ncarried items (" + items.Count + "/" + inventorySize + ")";
		foreach(Item i in items)
			printme += "\n   " + i.displayName;
		Debug.Log(printme);
	}

	public IEnumerable<Item> EquipmentEnumerator
	{
		get
		{
			return items;
		}
	}

	void UseAction(int turns)
	{
		for (int j = 0; j < turns; j++)
			GetComponent<CharacterActionController>().StackAction(CharacterActionController.Actions.inventoryaction);
	}

	public bool Contains(string s)
	{
		foreach(Item i in items)
		{
			if (i.displayName.ToLower() == s.ToLower())
				return true;
		}
		for(int i = 0; i < SLOTS; i++)
		{
			if(consumables[i, 0] != null)
			{
				if (consumables[i, 0].displayName.ToLower() == s.ToLower())
					return true;
			}
		}
		return false;
	}

	public int Quantity(string s)
	{
		int qty = 0;

		foreach (Item i in items)
		{
			if (i.displayName.ToLower() == s.ToLower())
				qty++;
		}
		for (int i = 0; i < SLOTS; i++)
		{
			for (int j = 0; j < STACKS; j++)
			{
				if (consumables[i, j] != null)
				{
					if (consumables[i, j].displayName.ToLower() == s.ToLower())
						qty++;
				}
			}
		}

		return qty;
	}

	public bool RemoveItem(string s)
	{
		foreach (Item i in items)
		{
			if (i.displayName.ToLower() == s.ToLower())
			{
				return DestroyItem(i);
			}
		}
		for (int i = 0; i < SLOTS; i++)
		{
			if (consumables[i, 0] != null)
			{
				if (consumables[i, 0].displayName.ToLower() == s.ToLower())
				{
					return RemoveConsumable(consumables[i, 0]);
				}
			}
		}
		return false;
	}

	public class InventoryEvent : UnityEvent<Inventory> { }
	public class ItemEvent : UnityEvent<Item> { }
	public class EquipEvent : UnityEvent<Equipment, EquipmentType> { }
	public class ConsumableEvent : UnityEvent<Item, int, int> { }
}
