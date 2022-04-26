///Author: Phap Nguyen.
///Description: Pickup and add item to the inventory.
///Day created: 22/03/2022
///Last edited: 24/04/2022 - Ly Duong Huy

using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Pickup : MonoBehaviour, Interactable
{
    [SerializeField] Transform playerPos; //Store player's position for movements towards the player
    [SerializeField] List<PickupList> pickupList;
    [SerializeField] Rigidbody rb; //Rigidbody ref
    [SerializeField] float movingForce; //amount of force apply on the item for it to move towards the character
    private bool moving = false; //boolean for turning the movements of the item on/off
    [SerializeField] SpriteRenderer graphic;

    

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

    private void Awake()
    {
        playerPos = GameObject.Find("Player").transform; //populate variable with target position
        rb = gameObject.GetComponent<Rigidbody>(); //auto find and populate the variable

    }

    private void Start()
    {
        moving = false;//set item movement state to pose at start
    }

    private void Update()
    {
        if (moving) CalculateForce(); //call move function
    }

    /// <summary>
    /// Used to calculate and apply force on items to make items move towards player when appoarching
    /// </summary>
    private void CalculateForce()
    {
        //Subtracting target pos to this object pos to get direction.
        Vector3 f = playerPos.position - this.transform.position;
        f = f.normalized; //normalize to make it a direction
        f = f * movingForce; //Make the force
        rb.AddForce(f); //apply the force to the item
    }

    private void OnTriggerEnter(Collider triggerCollide)
    {
        //When player enters, follow them.
        if(triggerCollide.name == "Player")
        {
            moving = true;

        }
        //if not stay still
        else
        {
            moving = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO: change from getting the GameManager to getting from the character's prefab.
        var inventory = FindObjectOfType<PlayerEntity>().GetComponent<Inventory>();

        if (collision.collider.name == "Player")
        {
            //Loop through the list of potential pickup.
            //And add them into the inventory.
            for (int i = 0; i < pickupList.Count; i++)
            {
                inventory.AddItem(pickupList[i].Item, pickupList[i].ItemCount);
            }

            Destroy(this.gameObject);

            //Add the item into inventory and destroy this gameobject
        }
    }

    void OnValidate()
    {
        graphic = GetComponentInChildren<SpriteRenderer>();
        if (pickupList.Count == 0) return;

        if(pickupList.Count > 1)
        {
            graphic.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Assets/UI/pickup_pouch.png", typeof(Sprite));
        }
        else if(pickupList.Count == 1)
        {
            graphic.sprite = pickupList[0].Item.ItemSprite;
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