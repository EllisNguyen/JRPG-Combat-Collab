using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Ly Duong Huy
 * Scripts: DialogueCharacter
 * Object: Sphere
 * Summary:
 * Handles dialogues
 */
public class DialogueCharacter : MonoBehaviour
{
    //dialogue for this specific class
    public Dialogue dialogue;
    bool isInRange = false;
    [SerializeField] GameObject textPopUp;

    //If player is in the trigger of the character, check for initiate dialogue input
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            isInRange = true;
            textPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag =="Player")
        {
            isInRange = false;
            textPopUp.SetActive(false);
        }
    }

    private void Update()
    {
        if(isInRange)
        {
            InitiateDialogue();
        }
    }


    //Check for E to Initiate the dialogue
    //called in OnMouseDown()
    void InitiateDialogue()   
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            textPopUp.SetActive(false);
        }
        
    }
    
}
