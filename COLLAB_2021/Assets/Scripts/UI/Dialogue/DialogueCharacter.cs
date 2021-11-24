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

    //When this shpere is clicked initiate the dialogue
    private void OnMouseDown()
    {
        InitiateDialogue();
    }

    //Initiate the dialogue
    //called in OnMouseDown()
    void InitiateDialogue()   
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
