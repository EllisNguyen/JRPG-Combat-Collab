using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
Author: Ly Duong Huy
Date: 10 Oct 2021
Attached to: Enemy
Summary:
Simple Enemy Follow the player Script
 */

public class EnemyMovement : MonoBehaviour, EnemyInterface
{
    [SerializeField] Transform playerTransform; //stores the player's Transform in the Editor
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] GameObject popupObject; //Store the damage popup object for instantiation
    public int ID { get; set; } //ID of the enemy. Stays inside of enemy movements for TESTING
    private DamagePopup damagePopup; //reference to the DamagePopup class
    EnemyInterface instance;
    public static event Action<EnemyInterface> enemyDead;

    bool follow = true; //enables following player or not

    private void Start()
    {
        ID = 0; //Set enemy ID TESTING
        instance = this.gameObject.GetComponent<EnemyInterface>();
    }


    // Update is called once per frame
    void Update()
    {
        if (follow == true)
        {
            FollowPlayer();// Execute if player is not colliding with the enemy
        }

    }

    private void OnMouseDown()
    {
        PopupValue(100);
    }

    private void PopupValue(float damage)
    {
        
        //instantiate popups and get their DamagePopup class
        damagePopup = Instantiate(popupObject, transform.position, Quaternion.identity).GetComponent<DamagePopup>();

        //Set the damage value the stated here
        damagePopup.SetDamageText(damage);
    }

    //invoke event to the quest counter and destroy this gameobject
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyDead?.Invoke(instance);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            follow = true; //Set follow to true when player enters the enemy trigger
            

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            follow = false; //Set follow to false when player exit the enemy trigger

        }
    }

    void FollowPlayer()
    //follow player function using MoveTowards command
    {
        Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
