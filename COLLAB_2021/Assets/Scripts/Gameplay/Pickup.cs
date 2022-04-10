///Author: Phap Nguyen.
///Description: Pickup and add item to the inventory.
///Day created: 22/03/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using UnityEngine;

public class Pickup : MonoBehaviour, Interactable
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public void Interact()
    {
        //TODO: change from getting the GameManager to getting from the character's prefab.
        var inventory = FindObjectOfType<GameManager>().GetComponent<Inventory>();

        inventory.AddItem(item, count);
    }
}
