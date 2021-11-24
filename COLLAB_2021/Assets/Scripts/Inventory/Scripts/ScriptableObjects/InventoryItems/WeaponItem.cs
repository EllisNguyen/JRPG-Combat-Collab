using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Equipment", menuName = "Inventory/Items/Equipments/Weapon")]
public class WeaponItem : ItemObject
{
    private void Awake()
    {
        itemType = ItemType.Weapon;

        atttributes = new int[10]; //first 9 ints correspond with the 10 character stats, last int is for type
    }
}
