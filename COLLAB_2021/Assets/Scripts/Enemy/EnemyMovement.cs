using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Author: Ly Duong Huy
Date: 10 Oct 2021
Attached to: Enemy
Summary:
Simple Enemy Follow the player Script
 */

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform playerTransform; //stores the player's Transform in the Editor
    [SerializeField] float movementSpeed = 6f;

    bool follow = true; //enables following player or not

    //example of subscribing to event in GameManager
   /* void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState currentState)
    {


        if (currentState == GameState.CombatPhase)
        {
            //Destroy this game object when it is combat phase
            Destroy(this.gameObject);
        }

    }
   */


    // Update is called once per frame
    void Update()
    {
        if (follow == true)
        {
            FollowPlayer();// Execute if player is not colliding with the enemy
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            follow = false; //Set follow to false when player enters the enemy trigger
            

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            follow = true; //Set follow to true when player exit the enemy trigger

        }
    }

    void FollowPlayer()
    //follow player function using MoveTowards command
    {
        Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
