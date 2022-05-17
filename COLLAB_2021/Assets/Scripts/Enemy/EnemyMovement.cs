using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System;
using Random = UnityEngine.Random;

/*
Author: Ly Duong Huy
Date: 10 Oct 2021
Attached to: Enemy
Summary:
Simple Enemy Follow the player Script
 */

public class EnemyMovement : Navigation
{
    public Transform playerTransform; //stores the player's Transform in the Editor
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] GameObject popupObject; //Store the damage popup object for instantiation
    //public int ID { get; set; } //ID of the enemy. Stays inside of enemy movements for TESTING
    private DamagePopup damagePopup; //reference to the DamagePopup class
    //EnemyInterface instance; //instance of the enemyinterface

    public bool follow = true; //enables following player or not

    //public NavMeshAgent enemy;

    private void Start()
    {
        Agent.speed = movementSpeed;
    }

    public virtual void HandleUpdate()
    {
        base.HandleUpdate();

        if (follow == true && playerTransform != null)
        {
            Agent.SetDestination(playerTransform.position);
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

    public Vector3 GetMovementDirection()
    {
        Vector3 normalizedDirection = Agent.desiredVelocity.normalized;
        return normalizedDirection;
    }
}
