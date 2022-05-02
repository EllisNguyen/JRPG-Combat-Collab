using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/*
 * Author: Ly Duong Huy
 * Scripts: DialogueCharacter
 * Object: Sphere
 * Summary:
 * Handles dialogues. Search for player using TAG.
 */
public class DialogueCharacter : MonoBehaviour, NPCInterface
{
    //dialogue for this specific class
    public Dialogue dialogue;
    
    public DialogueManager dialogueManager;
    [SerializeField] public GameObject textPopUp;

    public int npcIdNumber; //ID number

    public int ID { get; set; } //NPC ID for Quest identification
    public static event Action<NPCInterface> npcIDcheck;

    public NPCInterface instance; //instance NPCinterface
    private void Start()
    {
        
        instance = this; //refer the interface instance to this
        instance.ID = npcIdNumber; //Set this npc's ID
        //Get the dialogue manager
        dialogueManager = FindObjectOfType<DialogueManager>();
        textPopUp.SetActive(false);
    }

    //If player is in the trigger of the character, check for initiate dialogue input
    private void OnTriggerEnter(Collider collider)
    {
        
        //Player is in the talking range, make the popup appears and enable the player to start the dialogue
        if (collider.gameObject.tag == "Player")
        {
            
            textPopUp.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //start dialogue if player is not in another dialogue and has pressed E
            if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.inDialogue)
            {
                npcIDcheck?.Invoke(instance); //Trigger event

                InitiateDialogue();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //player is no longer in the talking range, disable the popup and do not let the player start conversation
        if(other.gameObject.tag =="Player")
        {
            
            textPopUp.SetActive(false);
        }
    }

    private void Update()
    {
        
        
    }


    //Run initiate Dialogue
    //called in OnMouseDown()
    public virtual void InitiateDialogue()   
    {
        
        
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        textPopUp.SetActive(false);

    }
    
}
