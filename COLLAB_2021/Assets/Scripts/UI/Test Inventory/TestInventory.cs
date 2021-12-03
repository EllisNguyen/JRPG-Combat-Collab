using System;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [SerializeField] List<ItemSlott> slots;
    public List<ItemSlott> SLOTS => slots;

    public static TestInventory GetInventory()
    {
        //????
        return FindObjectOfType<TestInventory>().GetComponent<TestInventory>();
    }
}

[Serializable]
public class ItemSlott
{
    [SerializeField] TestInvent_Item item;
    [SerializeField] int count;

    public TestInvent_Item ITEM => item;
    public int COUNT => count;
}
