using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Author: Huynh Vu Long Hai
///Description: Holds an inventory. Can add, remove, and check for items. Codes sourced from Coding With Unity's YouTube video: Unity3D - Scriptable Object Inventory System | Part 1
///Day created: 09/10/2021
///Last edited: 09/10/2021 - Hai

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory/New Inventory")]
public class InventoryScriptableObject : ScriptableObject
{
    public List<InventorySlot> inventory = new List<InventorySlot>();

    /// <summary>
    /// Adds an item and its stack to the inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="stack"></param>
    public void AddItem(ItemObject item, int stack)
    {
        if (item.stackable) //if item can be stacked
        {
            foreach (InventorySlot slot in inventory) //checks if item already exists in the inventory
            {
                if (slot.item == item) //if it does, adds to the existing slot
                {
                    slot.AddToStack(stack);

                    return; //terminates after adding
                }
            }
        }

        inventory.Add(new InventorySlot(item, stack)); //if item doesn't exist in inventory or isn't stackable, creates a new slot in the inventory for it
    }

    /// <summary>
    /// Removes an item by a certain amount from the inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="stack"></param>
    public void RemoveItem(ItemObject item, int stack)
    {
        foreach (InventorySlot slot in inventory) //checks if item exists in the inventory
        {
            if (slot.item == item) //if it does, remove from the stack
            {
                slot.RemoveFromStack(stack);

                if (slot.stack <= 0) //checks if there is any of that item in the stack
                {
                    inventory.Remove(slot); //removes empty stack
                }

                return; //terminates after removing
            }
        }
    }

    /// <summary>
    /// Checks if an item exists in a certain amount in the inventory.
    /// Returns true if the item does exist at the desired amount, false otherwise.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="stack"></param>
    /// <returns></returns>
    public bool CheckForItem(ItemObject item, int stack)
    {
        foreach (InventorySlot slot in inventory)
        {
            if (slot.item == item && slot.stack >= stack)
            {
                return true;
            }
        }

        return false;
    }
}

/// <summary>
/// Holds an item and its number in the inventory
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int stack = 0;

    public InventorySlot(ItemObject newItem, int newStack)
    {
        item = newItem;

        stack = newStack;
    }

    public void AddToStack(int numberToAdd)
    {
        stack += numberToAdd; 
    }

    public void RemoveFromStack(int numberToRemove)
    {
        stack -= numberToRemove;

        if (stack < 0)
        {
            stack = 0;
        }
    }
}
