using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Other Consumable", menuName = "Inventory/Items/Consumables/Other")]
public class OtherItem : ItemObject
{
    private void Awake()
    {
        itemType = ItemType.Other;
    }
}
