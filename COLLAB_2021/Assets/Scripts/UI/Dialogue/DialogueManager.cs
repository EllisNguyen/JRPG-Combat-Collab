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
    [SerializeField] GameObject dialogueContainer; //reference to the dialogue container
    [SerializeField] TextMeshProUGUI displayName; //reference for name text
    [SerializeField] TextMeshProUGUI displayDialogue; //reference for dialogue text
    [SerializeField] GameObject continueButton; //reference to the continue button
    [SerializeField] Queue<string> sentences; //queue for sentences in a dialogue
    

    public bool inDialogue = false;

    private void Awake()
    {
        //displayName = GameObject.Find("Speaker_Txt").GetComponent<TextMeshProUGUI>();
        //displayDialogue = GameObject.Find("Dialogue_Txt").GetComponent<TextMeshProUGUI>();
        //continueButton = GameObject.Find("Continue");
        //continueButton = GameObject.Find("Dialogue_Container");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the queue
        sentences = new Queue<string>();

        //DISABLE THE WHOLE CONTAINER NOW, DON'T NEED TO SET TEXT COLOR ANYMORE
        //displayName.faceColor = new Color32(0, 0, 0, 0);
        //displayDialogue.faceColor = new Color32(0, 0, 0, 0);

        dialogueContainer.SetActive(false);
        continueButton.SetActive(false);

    }

    
    //Start the dialogue
    public void StartDialogue(Dialogue dialogue)
    {
        //DISABLE THE WHOLE CONTAINER NOW, DON'T NEED TO SET TEXT COLOR ANYMORE
        //displayName.faceColor = new Color32(0, 0, 0, 255);
        //displayDialogue.faceColor = new Color32(0, 0, 0, 255);

        //Enable everything when start dialogue
        dialogueContainer.SetActive(true);
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
        GameManager.Instance.gameState = GameState.Dialogue;
    }

    //Added by Phap -> ref by GameManager, whenever the dialogue is active, player can push e to continue dialogue.
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dequeue();
        }
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
        //displayDialogue.text = "end";
        inDialogue = false;
        //Set the color of the dialogue UI to transparent and hide the continue button
        //After the conversation ended

        //DISABLE THE WHOLE CONTAINER NOW, DON'T NEED TO SET TEXT COLOR ANYMORE
        //displayName.faceColor = new Color32(0, 0, 0, 0);
        //displayDialogue.faceColor = new Color32(0, 0, 0, 0);


        continueButton.SetActive(false);
        dialogueContainer.SetActive(false);

        GameManager.Instance.gameState = GameState.FreeRoam;
    }
}
