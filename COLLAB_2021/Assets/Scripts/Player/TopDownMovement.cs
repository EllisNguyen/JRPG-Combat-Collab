using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Author: Ly Duong Huy
Date: 05 Oct 2021
Attached to: Player
Summary:
Handles Player's Movements
 */

public class TopDownMovement : MonoBehaviour
{
    //Variables
    Transform cameraTransform;

    
    [SerializeField] float speed = 6f; //player movement speed
    [SerializeField] float sprintSpeed = 10f; //player sprint speed


    //Creates an instance to reference
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform; //get camera reference
    }

    // Update is called once per frame
    void Update()
    {
        
        
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

        Vector3 playerPosition = this.transform.position;//store the position before moving

        float playerAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        //returns the deviation in RADIAN from z axis (Atan2 x/z), 
        //exchange to degrees through Rad2Deg 
        //and add in camera angles for camera dependency.

        Vector3 movements = Quaternion.Euler(0f, playerAngle, 0f) * Vector3.forward; //movements with the camera offset taken into calculation

        if (Input.GetKey(KeyCode.LeftShift))
        //sprint speed
        {

            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                playerPosition += movements * Time.deltaTime * sprintSpeed;
                //movements
            }
        }
        else if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        //normal speed
        {

            playerPosition += movements * Time.deltaTime * speed;
            //movements
        }
        else
        {
            //animation idle
        }

        this.transform.position = playerPosition; //updating the position after moving

    }
}
