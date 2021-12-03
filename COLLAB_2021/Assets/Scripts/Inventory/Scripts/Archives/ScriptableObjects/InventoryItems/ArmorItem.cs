using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Equipment", menuName = "Inventory/Items/Equipments/Armor")]
public class ArmorItem : ItemObject
{
    private void Awake()
    {
        itemType = ItemType.Armor;

        atttributes = new int[10]; //first 9 ints correspond with the 10 character stats, last int is for type
    }
    

}
