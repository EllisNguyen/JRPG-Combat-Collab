using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;
    public List<ItemSlot> SLOTS => slots;

    public static TestInventory GetInventory()
    {
        //????
        return FindObjectOfType<TestInventory>().GetComponent<TestInventory>();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] TestInvent_Item item;
    [SerializeField] int count;

    public TestInvent_Item ITEM => item;
    public int COUNT => count;
}
