using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Ly Duong Huy
 * Scripts: DialogueCharacter
 * Object: Sphere
 * Summary:
 * Handles dialogues. Search for player using TAG.
 */
public class DialogueCharacter : MonoBehaviour
{
    //dialogue for this specific class
    public Dialogue dialogue;
    bool isInRange = false;
    private DialogueManager dialogueManager;
    [SerializeField] GameObject textPopUp;

    private void Start()
    {
        //Get the dialogue manager
        dialogueManager = FindObjectOfType<DialogueManager>();
        textPopUp.SetActive(false);
    }

    //If player is in the trigger of the character, check for initiate dialogue input
    private void OnTriggerEnter(Collider collider)
    {
        //Player is in the talking range, make the popup appears and enable the player to start the dialogue
        if(collider.gameObject.tag == "Player")
        {
            isInRange = true;
            textPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //player is no longer in the talking range, disable the popup and do not let the player start conversation
        if(other.gameObject.tag =="Player")
        {
            isInRange = false;
            textPopUp.SetActive(false);
        }
    }

    private void Update()
    {
        //Check for player's input to start dialogue
        if(isInRange)
        {
            InitiateDialogue();
        }
        
    }


    //Check for E to Initiate the dialogue
    //called in OnMouseDown()
    public void InitiateDialogue()   
    {
        //start dialogue if player is not in another dialogue and has pressed E
        if(Input.GetKeyDown(KeyCode.E) && !dialogueManager.inDialogue)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            textPopUp.SetActive(false);
        }
        
    }
    
}
