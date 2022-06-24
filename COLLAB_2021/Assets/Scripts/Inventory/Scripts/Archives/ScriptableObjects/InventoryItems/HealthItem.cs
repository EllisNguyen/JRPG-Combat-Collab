using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Consumable", menuName = "Inventory/Items/Consumables/Health")]
public class HealthItem : ItemObject
{
    private Character character;
    private void Awake()
    {
        itemType = ItemType.Health;

        atttributes = new int[2]; //first int is for the amount to heal, second is to pass in the duration if it heals over time
    }
    public void Use()
    {
        character.HP += atttributes[1];
        
    }
   
}
