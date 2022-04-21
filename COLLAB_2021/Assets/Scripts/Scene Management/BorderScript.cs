using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Class: BorderScript
    Date: 21/04/2022
    Author: Ly Duong Huy
    Object: Border (Area Transitioning gameObject)
    Summary:
    Issues the command to change the scene when the player touches the border.
    */

public class BorderScript : MonoBehaviour
{

    //Variables
    [SerializeField] string targetSceneName;


    //On Player collision, load the scene assigned to the variable
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Player")
        {
            SceneTransition.instance.LoadScene(targetSceneName);
        }
    }
}
