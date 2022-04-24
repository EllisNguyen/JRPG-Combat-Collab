using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create consumable item")]
public class Item_Consumable : ItemBase
{
    public override bool Use(Character character)
    {
        return false;
    }

    //TODO: add more custom behaviours for the consumable here.

    /*
    Custom behaviours like: (example)
    -What happen when use the item: maybe display a dialogue to indicate that the item has been used.
    -
    */
}
