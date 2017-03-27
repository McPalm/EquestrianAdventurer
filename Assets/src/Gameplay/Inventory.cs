using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

// an inventory for characters, equipment and other good stuffs
public class Inventory : MonoBehaviour
{
	public int inventorySize = 12;

	public InventoryEvent EventChangeEquipment = new InventoryEvent();
	public ItemEvent EventRemoveItem = new ItemEvent();
	public ItemEvent EventDropItem = new ItemEvent();
	public ItemEvent EventAddItem = new ItemEvent();
	public EquipEvent EventEquipItem = new EquipEvent();
	/// may fire before or after the item is removed from the slot. Only EventChangeEquipment is guarenteed to fire after the fact.
	public EquipEvent EventUnEquipItem = new EquipEvent();


	List<Item> items = new List<Item>(6);

	Equipment bodySlot;
	Equipment weaponSlot;

	public bool EmptySpace
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

	/// <summary>
	/// Picking the first item on the ground
	/// </summary>
	/// <returns>true if there is an item and it can be picked up</returns>
	public bool PickupFromGround()
	{
		if (!EmptySpace) return false;
		GroundItem g = null;
		foreach(MapObject o in ObjectMap.Instance.ObjectsAtLocation(GetComponent<MapObject>().RealLocation))
		{
			g = o.GetComponent<GroundItem>();
			if (g) break;
		}
		if(g && AddItem(g.item))
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
	public bool AddItem(Item i)
	{
		if (!EmptySpace) return false;
		items.Add(i);
		EventAddItem.Invoke(i);
		return true;
	}

	/// <summary>
	/// Remove an item from the inventory
	/// </summary>
	/// <param name="i">the item to remove</param>
	/// <returns>true if the item is in the inventory</returns>
	public bool RemoveItem(Item i)
	{
		
		if(items.Remove(i))
		{
			EventRemoveItem.Invoke(i);
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
		EventDropItem.Invoke(i);
		if(RemoveItem(i))
		{
			// make the item and put on ze gorund
			GameObject o = new GameObject(i.displayName);
			o.transform.position = (Vector3)GetComponent<MapObject>().RealLocation;
			o.AddComponent<GroundItem>().item = i;
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
		print("Try Equip " + e.displayName);
		switch(e.slots)
		{
			case EquipmentType.body:
				if (bodySlot != null) return false;
				RemoveItem(e);
				bodySlot = e;
				EventChangeEquipment.Invoke(this);
				EventEquipItem.Invoke(e, EquipmentType.body);
				print("Equipped " + e.displayName);
				return true;
			case EquipmentType.weapon:
				if (weaponSlot != null) return false;
				RemoveItem(e);
				weaponSlot = e;
				EventChangeEquipment.Invoke(this);
				EventEquipItem.Invoke(e, EquipmentType.weapon);
				print("Equipped " + e.displayName);
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
		if (drop) throw new System.NotImplementedException("Dropping items not implemented.");
		if (!drop && !EmptySpace) return false;
		switch (slot)
		{
			case EquipmentType.body:
				if (bodySlot == null) return false;
				AddItem(bodySlot);
				EventUnEquipItem.Invoke(bodySlot, EquipmentType.body);
				bodySlot = null;
				EventChangeEquipment.Invoke(this);
				return true;
			case EquipmentType.weapon:
				if (weaponSlot == null) return false;
				AddItem(weaponSlot);
				EventUnEquipItem.Invoke(weaponSlot, EquipmentType.weapon);
				weaponSlot = null;
				EventChangeEquipment.Invoke(this);
				return true;
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
				AddItem(bodySlot);
				bodySlot = null;
				EventUnEquipItem.Invoke(item, EquipmentType.body);
				EventChangeEquipment.Invoke(this);
				if (drop) DropItem(item);
				return true;
			case EquipmentType.weapon:
				if (weaponSlot != item) return false;
				AddItem(weaponSlot);
				weaponSlot = null;
				EventUnEquipItem.Invoke(item, EquipmentType.weapon);
				EventChangeEquipment.Invoke(this);
				if (drop) DropItem(item);
				return true;
		}
		return false;
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

	public class InventoryEvent : UnityEvent<Inventory> { }
	public class ItemEvent : UnityEvent<Item> { }
	public class EquipEvent : UnityEvent<Equipment, EquipmentType> { }
}
