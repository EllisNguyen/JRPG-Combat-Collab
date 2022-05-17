///Author: Phap Nguyen.
///Description: ScriptableObject for playable character.
///Day created: 25/04/2022
///Last edited: 17/05/2022 - Phap Nguyen.

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
