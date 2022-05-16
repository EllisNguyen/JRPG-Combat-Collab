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

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTransform; //stores the player's Transform in the Editor
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] GameObject popupObject; //Store the damage popup object for instantiation
    //public int ID { get; set; } //ID of the enemy. Stays inside of enemy movements for TESTING
    private DamagePopup damagePopup; //reference to the DamagePopup class
    //EnemyInterface instance; //instance of the enemyinterface

    public float wanderRadius;
    [MinMaxSlider(1, 10, true)] public Vector2 wanderTimer = new Vector2(3, 5);
    private float timer;


    public bool follow = true; //enables following player or not

    public NavMeshAgent enemy;

    private void Start()
    {
        //ID = 0; //Set enemy ID TESTING
        //instance = this.gameObject.GetComponent<EnemyInterface>();
    }

    public void HandleUpdate()
    {
        if(follow == false && playerTransform == null)
        {
            timer += Time.deltaTime;
            float nextTimer = Random.Range(wanderTimer.x, wanderTimer.y);

            if (timer >= nextTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                enemy.SetDestination(newPos);
                timer = 0;
            }
            return;
        }

        if (follow == true && playerTransform != null)
        {
            enemy.SetDestination(playerTransform.position);
        }

    }

    private void OnMouseDown()
    {
        PopupValue(100);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
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
        Vector3 normalizedDirection = enemy.desiredVelocity.normalized;
        return normalizedDirection;
    }

    private void OnDrawGizmos()
    {
        Vector3 gizmosPos = new Vector3((float)transform.position.x, (float)transform.position.y);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
