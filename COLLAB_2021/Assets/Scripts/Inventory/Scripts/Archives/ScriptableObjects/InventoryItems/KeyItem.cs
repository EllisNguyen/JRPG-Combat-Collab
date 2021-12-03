using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Inventory/Items/Key")]
public class KeyItem : ItemObject
{
    private void Awake()
    {
        itemType = ItemType.Key;
    }
}
