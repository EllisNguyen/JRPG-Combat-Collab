using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Author: Huynh Vu Long Hai
///Description: Holds item info
///Day created: 09/10/2021
///Last edited: 15/10/2021 - Hai

public enum ItemType
{
    Health,
    Stats,
    Weapon,
    Armor,
    Key,
    Other
}

public abstract class ItemObject : ScriptableObject
{
    [TextArea(10, 30)]
    public string itemDescription;

    public Sprite itemSprite;

    public ItemType itemType;

    public int value;

    public bool stackable;

    [Tooltip("Attributes are just the stats of the item. Like a health item will only use the first attribute in the array to determine restoration value, or a weapon will use like 4 to determine damage modifier, element, accuracy modifier, and crit rate. Add as many as needed.")]
    public int[] atttributes = new int[5];
    public virtual void Use()
    {
        Debug.Log(name);
    }    
}

