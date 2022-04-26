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
    [SerializeField] GameObject continueButton; //reference to the continue button
    [SerializeField] Queue<string> sentences; //queue for sentences in a dialogue
    

    public bool inDialogue = false;

    private void Awake()
    {
        displayName = GameObject.Find("Speaker").GetComponent<TextMeshProUGUI>();
        displayDialogue = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
        continueButton = GameObject.Find("Continue");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the queue
        sentences = new Queue<string>();

        //Set the color of the dialogue UI to transparent and hide the continue button
        displayName.faceColor = new Color32(0, 0, 0, 0);
        displayDialogue.faceColor = new Color32(0, 0, 0, 0);
        continueButton.SetActive(false);

    }

    
    //Start the dialogue
    public void StartDialogue(Dialogue dialogue)
    {

        //Enable everything when start dialogue
        displayName.faceColor = new Color32(0, 0, 0, 255);
        displayDialogue.faceColor = new Color32(0, 0, 0, 255);
        continueButton.SetActive(true);

        //Change the name text into the speaker's name
        displayName.text = dialogue.speaker;

        //Clear the queue to make sure nothing is in there
        if (sentences == null) sentences = new Queue<string>();
        else sentences.Clear();

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
    public void EndDialogue()
    {
        displayDialogue.text = "end";
        inDialogue = false;
        //Set the color of the dialogue UI to transparent and hide the continue button
        //After the conversation ended
        displayName.faceColor = new Color32(0, 0, 0, 0);
        displayDialogue.faceColor = new Color32(0, 0, 0, 0);
        continueButton.SetActive(false);
    }
}
