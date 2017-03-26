using UnityEngine;
using UnityEngine.Events;
using System.Collections;

// an inventory for characters, equipment and other good stuffs
public class Inventory : MonoBehaviour
{
	public int inventorySize = 12;

	public InventoryEvent EventChangeEquipment = new InventoryEvent();
	public ItemEvent EventRemoveItem = new ItemEvent();
	public ItemEvent EventDropItem = new ItemEvent();
	public ItemEvent EventAddItem = new ItemEvent();
	public EquipEvent EventEquipItem = new EquipEvent();
	public EquipEvent EventUnEquipItem = new EquipEvent();

	/// <summary>
	/// Add an item to the inventory
	/// </summary>
	/// <param name="i">Item to put in inventory</param>
	/// <returns>true of the change went through</returns>
	public bool AddItem(Item i)
	{
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// Remove an item from the inventory
	/// </summary>
	/// <param name="i">the item to remove</param>
	/// <returns>true if the item is in the inventory</returns>
	public bool RemoveItem(Item i)
	{
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// Drop the given item to the ground
	/// </summary>
	/// <param name="i">the item in question</param>
	/// <returns>true if we could drop it</returns>
	public bool DropItem(Item i)
	{
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// Equip an item into an appropiate slot
	/// trying to equip into an already used slot fails automaticall
	/// </summary>
	/// <param name="e"></param>
	/// <returns>true if equipped succesfully</returns>
	public bool EquipItem(Equipment e)
	{
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// Remove the item in the given slot
	/// </summary>
	/// <param name="slot">The slot wich an item will be removed from</param>
	/// <param name="drop">if true, put on the ground, if false, put in inventory</param>
	/// <returns>true if we succesfully removed the item from the slot</returns>
	public bool UnEquip(EquipmentType slot, bool drop = false)
	{
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// Remove the given item from the slot that it is in
	/// </summary>
	/// <param name="item">the item to remove</param>
	/// <param name="drop">if true, put on the ground, if false, put in inventory</param>
	/// <returns>true if the item could be removed from the inventory</returns>
	public bool UnEquip(Equipment item, bool drop = false)
	{
		throw new System.NotImplementedException();
	}


	public class InventoryEvent : UnityEvent<Inventory> { }
	public class ItemEvent : UnityEvent<Item> { }
	public class EquipEvent : UnityEvent<Equipment, EquipmentType> { }
}
