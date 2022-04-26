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
    [SerializeField] TutorialDialogue newspaperDialogues; //ref to the inventoryDialogue Obj
    private DialogueManager dialogueManager; //ref to the dialogue manager


    // Start is called before the first frame update
    void Start()
    {
        //Get the dialogue manager
        dialogueManager = FindObjectOfType<DialogueManager>(); //auto populate the variable
        StartCoroutine(MovementDialogueInitiate()); //initiate a Delay before the tutorial start

    }

    // Update is called once per frame
    void Update()
    {
        //Removes the tutorial dialogue upon movement
        if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal") && movementDialogues.inDialogue)
        {
            
            dialogueManager.EndDialogue();
            
        }
        
            
        
    }

    IEnumerator MovementDialogueInitiate()
    {
        yield return new WaitForSeconds(4f); //Delay for 4s

        //Start conversartion with God
        dialogueManager.StartDialogue(movementDialogues.dialogue);
        movementDialogues.inDialogue = true;
    }

    IEnumerator ReachNewsPaper()
    {
        yield return new WaitForSeconds(2f);//Delay 2s
        dialogueManager.StartDialogue(newspaperDialogues.dialogue);
        newspaperDialogues.inDialogue = true;
    }
}
