///Author: Phap Nguyen.
///Description: Pickup and add item to the inventory.
///Day created: 22/03/2022
///Last edited: 21/04/2022 - Phap Nguyen.

using UnityEngine;
using System.Collections.Generic;

public class Pickup : MonoBehaviour, Interactable
{
    public List<ItemList> list;

    //TODO: call Interact() method when player enter trigger => auto pickup on trigger.

    /// <summary>
    /// Interact method that get the inventory and put the list of item into it.
    /// </summary>
    public void Interact()
    {
        //TODO: change from getting the GameManager to getting from the character's prefab.
        var inventory = FindObjectOfType<PlayerEntity>().GetComponent<Inventory>();

        //Loop through the list of items in ItemList and add them to inventory.
        for (int i = 0; i < list.Count; i++)
        {
            inventory.AddItem(list[i].Item, list[i].ItemCount);
            print("picked up " + list[i].ItemCount + " " + list[i].Item);
        }
    }
}

//Serializable class that give 2 var of type ItemBase and int.
//Reference in Pickup, to create a list of pickable object(s).
[System.Serializable]
public class ItemList
{
    [SerializeField] ItemBase item;
    [Range(1,10)][SerializeField] int itemCount;

    public ItemBase Item => item;
    public int ItemCount => itemCount;
}