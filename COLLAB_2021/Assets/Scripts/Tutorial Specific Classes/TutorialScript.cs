using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Author: Ly Duong Huy
 * Class: TutorialScript
 * Date: 25/04/2002
 * Obj: 
 * Summary: Provisional tutorial
 */


public class TutorialScript : MonoBehaviour
{
    //Variables
    [SerializeField] TutorialDialogue movementDialogues; //ref to the movementDialogue Obj
    private DialogueManager dialogueManager; //ref to the dialogue manager


    // Start is called before the first frame update
    void Start()
    {
        //Get the dialogue manager
        dialogueManager = FindObjectOfType<DialogueManager>(); //auto populate the variable
        StartCoroutine(Delay()); //initiate a Delay before the tutorial start

    }

    // Update is called once per frame
    void Update()
    {
        //Removes the tutorial dialogue upon movement
        if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal") && movementDialogues.inDialogue)
        {
            dialogueManager.Dequeue();
        }
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f); //Delay for 2s

        //Start conversartion with God
        dialogueManager.StartDialogue(movementDialogues.dialogue);
        movementDialogues.inDialogue = true;
    }
}
