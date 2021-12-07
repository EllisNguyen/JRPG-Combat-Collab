using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemCatergory { Medicines, Weapons, Armours };

public class Inventory : MonoBehaviour
{
    //Declare the list of all item type.
    [SerializeField] List<ItemSlot> medicineSlots;
    [SerializeField] List<ItemSlot> weaponDeviveSlots;
    [SerializeField] List<ItemSlot> armourSlots;

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { medicineSlots, weaponDeviveSlots, armourSlots };
    }

    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "MEDICINES", "WEAPONS", "ARMOURS"
    };

    //Function return 1 of 3 list category.
    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
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
        var item = currentSlot[itemIndex].ITEM;
        bool itemUsed = item.Use(selectedCharacter);

        //Do the usage thingy lol.
        if (itemUsed)
        {
            RemoveItem(item, selectedCategory);
            return item;
        }

        return null;
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
        var itemSlot = currentSlot.First(slot => slot.ITEM == item);

        //Decrease on use.
        itemSlot.COUNT--;

        //Check for item availability.
        //Remove if reached 0.
        if (itemSlot.COUNT == 0)
            currentSlot.Remove(itemSlot);

        //Update UI.
        OnUpdated?.Invoke();
    }

    //Simply expose the inventory.
    public static TestInventory GetInventory()
    {
        return FindObjectOfType<TestInventory>().GetComponent<TestInventory>();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public ItemBase ITEM => item;
    public int COUNT { get => count; set => count = value; }
}
