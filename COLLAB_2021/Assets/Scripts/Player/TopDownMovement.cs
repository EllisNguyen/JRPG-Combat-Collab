using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
Author: Ly Duong Huy
Date: 05 Oct 2021
Attached to: Player
Summary:
Handles Player's Movements

Last edit: 07/05/2022 - by Phap Nguyen.
 */

[RequireComponent(typeof(NavMeshAgent))]
public class TopDownMovement : MonoBehaviour
{
    //Variables
    Transform cameraTransform;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] float speed = 6f; //player movement speed

    [SerializeField] AlterAnim animator;
    [SerializeField] bool isMoving;


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform; //get camera reference
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Movements for player based on Vector.
    /// Located in TopDownMovement Class.
    /// </summary>
    public void Movements()
    //Call in Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //Get horizontal input without smoothing
        float vertical = Input.GetAxisRaw("Vertical");//Get vertical input without smoothing
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; //Input data stored into a Vector and normalized

        if (direction.x != 0)
        {
            animator.MoveX = direction.x;
            if (direction.x > 0) animator.FlipSprite(false);
            else animator.FlipSprite(true);
        }

        Vector3 playerPosition = this.transform.position;//store the position before moving

        //returns the deviation in RADIAN from z axis (Atan2 x/z), 
        //exchange to degrees through Rad2Deg 
        //and add in camera angles for camera dependency.
        
        if (direction != Vector3.zero)
        {
            //playerPosition += movements * Time.deltaTime * speed;
            //movements
            Vector3 destination = transform.position + transform.right * direction.x + transform.forward * direction.z;

            agent.speed = speed;
            agent.destination = destination;
            isMoving = true;
        }
        else
        {
            agent.destination = transform.position;

            //animation idle
            isMoving = false;
        }

        animator.IsMoving = isMoving;
        this.transform.position = playerPosition; //updating the position after moving
    }

    public void ResetNavMesh()
    {
        agent.ResetPath();
    }
}
