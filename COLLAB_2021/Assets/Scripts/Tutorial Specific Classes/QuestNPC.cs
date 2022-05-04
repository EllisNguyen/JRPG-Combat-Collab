using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Author: Ly Duong Huy
 * Class: QuestNPC
 * Date: 27/04/2022
 * Summary: QuestNPC interactions
 */

public class QuestNPC : DialogueCharacter
{
    public bool assignedQuest; //determine whether the quest has been assigned to the player or not
    public bool helped; //determine whether the player has finished the quest

    private QuestManager questManager;
    
    public Dialogue questCompletedDialogue;
    
    public Dialogue questNotCompletedDialogue;
    
    public Dialogue questAlreadyDoneDialogue;

    //[SerializeField] private GameObject quests;

    //[SerializeField] private string questType;

    private Quest quest;

    [SerializeField] int questNumber;

    [SerializeField] Quest questToAdd;
    //private QuestSystem quest;

    public  override void Start()
    {
        base.Start();
        questManager = FindObjectOfType<QuestManager>();
    }
    public override void InitiateDialogue()
    {
        
        if (!assignedQuest && !helped)
        {
            base.InitiateDialogue();
            
            //assign the quest
            AssignQuest();
        }
        else if (assignedQuest && !helped)
        {
            //check
            CheckQuest();
        }
        else
        {
            
            if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.inDialogue)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(questAlreadyDoneDialogue);
                textPopUp.SetActive(false);
            }
        }
    }

    void AssignQuest()
    {
        

        assignedQuest = true;

        //passed in the quest in order to keep a reference to it. (TEST)
        //quest = (QuestSystem)quests.AddComponent(System.Type.GetType(questType));

        questManager.CurrentQuests.Add(questToAdd);
        quest = questManager.CurrentQuests[questNumber];
        questManager.CreateQuest();
        

    }

    void CheckQuest()
    {
        if (quest.Completed)
        {
            
            helped = true;
            assignedQuest = false;
            if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.inDialogue)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(questCompletedDialogue);
                textPopUp.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.inDialogue)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(questNotCompletedDialogue);
                textPopUp.SetActive(false);
            }
        }
    }
}
