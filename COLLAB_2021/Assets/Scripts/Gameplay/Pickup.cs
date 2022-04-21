///Author: Phap Nguyen.
///Description: Pickup and add item to the inventory.
///Day created: 22/03/2022
///Last edited: 21/04/2022 - Phap Nguyen.

using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interactable
{
    [SerializeField] List<PickupList> pickupList;

    public void Interact()
    {
        //TODO: change from getting the GameManager to getting from the character's prefab.
        var inventory = FindObjectOfType<PlayerEntity>().GetComponent<Inventory>();

        //Loop through the list of potential pickup.
        //And add them into the inventory.
        for (int i = 0; i < pickupList.Count; i++)
        {
            inventory.AddItem(pickupList[i].Item, pickupList[i].ItemCount);
        }
    }
}

//Serializable class call by Pickup class to create a list of item that will add to inventory when picking up the item.
[System.Serializable]
public class PickupList
{
    [SerializeField] ItemBase item;
    [Range(1,10)][SerializeField] int itemCount;

    //Publicly expose the local variables.
    public ItemBase Item => item;
    public int ItemCount => itemCount;
}