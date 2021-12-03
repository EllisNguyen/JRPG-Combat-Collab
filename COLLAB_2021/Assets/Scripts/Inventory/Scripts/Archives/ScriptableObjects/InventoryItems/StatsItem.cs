using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Consumable", menuName = "Inventory/Items/Consumables/Stats")]
public class StatsItem : ConsumableObject
{
    private void Awake()
    {
        itemType = ItemType.Stats;

        atttributes = new int[11]; //first 10 ints correspond with the 10 character stats to modify, last int is for duration
    }

}
