///Author: Phap Nguyen.
///Description: the main class of the Inventory, all categories, adding and removing item operation will be control by this class.
///Day created: 20/01/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemCatergory { Consumable, Armor, Weapon, QuestItem };

//THIS SCRIPT SHOULD BE ON THE SAME GAMEOBJECT WITH PLAYERCONTROLLER.
public class Inventory : MonoBehaviour
{
    //Declare the list of all item type.
    [SerializeField] List<ItemSlot> consumableSlots;
    [SerializeField] List<ItemSlot> armorSlots;
    [SerializeField] List<ItemSlot> weaponSlots;
    [SerializeField] List<ItemSlot> questItemSlots;

    //Add all list of item in one list.
    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { consumableSlots, armorSlots, weaponSlots, questItemSlots };
    }

    /// <summary>
    /// A list of string that will display the item category as string.
    /// </summary>
    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "CONSUMABLES", "ARMORS", "WEAPONS", "QUEST ITEMS"
    };

    /// <summary>
    /// Return the category by getting the index of the slot.
    /// </summary>
    /// <param name="categoryIndex"></param>
    /// <returns></returns>
    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    /// <summary>
    /// Get the sum items currently present in the Inventory.
    /// </summary>
    /// <returns></returns>
    public int GetTotalItemNumber()
    {
        int allConsumable = 0;
        int allArmor = armorSlots.Count;
        int allWeapon = weaponSlots.Count;

        for (int i = 0; i < consumableSlots.Count; i++)
        {
            allConsumable += consumableSlots[i].Count;
        }

        var itemNumber = allConsumable + allArmor + allWeapon;
        return itemNumber;
    }

    public int GetItemInSlot(int categoryIndex)
    {
        int itemCount = GetSlotsByCategory(categoryIndex).Count;

        return itemCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="selectedCreature"></param>
    /// <returns>   To indicate the usage of the item   </returns>
    public ItemBase UseItem(int itemIndex, Character selectedCharacter, int selectedCategory)
    {
        var currentSlot = GetSlotsByCategory(selectedCategory);

        //Get item information of the selected index in the inventory.
        //And apply item effect on the selected creature.
        var item = currentSlot[itemIndex].Item;
        bool itemUsed = item.Use(selectedCharacter);

        //Do the usage thingy lol.
        if (itemUsed)
        {
            RemoveItem(item, selectedCategory);
            return item;
        }

        return null;
    }

    ItemCatergory GetCategoryFromItem(ItemBase item)
    {
        if (item is Item_Consumable)
            return ItemCatergory.Consumable;
        else if (item is Item_Armor)
            return ItemCatergory.Armor;
        else if(item is Item_Weapon)
            return ItemCatergory.Weapon;
        else
            return ItemCatergory.QuestItem;
    }

    public void AddItem(ItemBase item, int count = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlot = GetSlotsByCategory(category);

        var itemSlot = currentSlot.FirstOrDefault(slot => slot.Item == item);
        if(itemSlot != null && item.Stackable)
        {
            itemSlot.Count += count;
        }
        else
        {
            currentSlot.Add(new ItemSlot()
            {
                Item = item,
                Count = count,
            });
        }
        OnUpdated?.Invoke();
    }

    /// <summary>
    /// Decrease item count on usage.
    /// Remove the item if item count is 0.
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(ItemBase item, int selectedCategory)
    {
        var currentSlot = GetSlotsByCategory(selectedCategory);

        //Ref to item slot.
        var itemSlot = currentSlot.First(slot => slot.Item == item);

        //Decrease on use.
        itemSlot.Count--;

        //Check for item availability.
        //Remove if reached 0.
        if (itemSlot.Count == 0)
            currentSlot.Remove(itemSlot);

        //Update UI.
        OnUpdated?.Invoke();
    }

    //Simply expose the inventory.
    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerEntity>().GetComponent<Inventory>();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;
    [SerializeField] int coreAttributeCount;

    public ItemBase Item { get => item; set => item = value; }
    public int Count { get => count; set => count = value; }
    public int CoreAttributeCount { get => coreAttributeCount; set => coreAttributeCount = value; }
}
