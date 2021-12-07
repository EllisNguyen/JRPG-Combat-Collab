using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // included to access the components of TextMeshPro
/*
 * Author: Ly Duong Huy
 * Scripts: DialogueManager
 * Object: DialogueManager
 * Summary:
 * Handles dialogues
 * Notes:
 * Set the Continue button in DialogueCanvas On Click() to Dequeue() so that the conversation can progress
 */
public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayName; //reference for name text
    [SerializeField] TextMeshProUGUI displayDialogue; //reference for dialogue text
    [SerializeField] Queue<string> sentences; //queue for sentences in a dialogue

    public bool inDialogue = false;

    private void Awake()
    {
        displayName = GameObject.Find("Speaker").GetComponent<TextMeshProUGUI>();
        displayDialogue = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the queue
        sentences = new Queue<string>();
    }

    
    //Start the dialogue
    public void StartDialogue(Dialogue dialogue)
    {
        //Change the name text into the speaker's name
        displayName.text = dialogue.speaker;

        //Clear the queue to make sure nothing is in there
        sentences.Clear();

        //Enqueue each sentence that are specified in DialogueCharacter holder
         foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Dequeue();
        inDialogue = true;

    }

    

    //display the next sentence
    //called in Update()
    public void Dequeue()
    {

        //Check if the end of the queue has been reached
        if(sentences.Count == 0)
        {
            //if true, end dialogue
            EndDialogue();
            return;
        }
        else
        {
            //else load the next sentence
            displayDialogue.text = sentences.Dequeue();
        }
        
    }

    //End the dialogue
    //called in Dequeue()
    void EndDialogue()
    {
        displayDialogue.text = "end";
        inDialogue = false;
    }
}
