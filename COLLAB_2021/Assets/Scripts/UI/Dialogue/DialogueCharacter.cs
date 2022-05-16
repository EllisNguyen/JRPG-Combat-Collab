using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;//TextMeshPro Lib
/*
 * Author: Ly Duong Huy
 * Scripts: DialogueCharacter
 * Object: Sphere
 * Summary:
 * Handles dialogues. Search for player using TAG.
 */
public class DialogueCharacter : MonoBehaviour, NPCInterface
{
    #region Variables
    float startTimer = 0;                                   //Start timer always 0.
    [Header("BUTTON HOLD TIMER")]
    [Range(0, 100)] [SerializeField] float endTimer = 2;     //The end float that conclude the holding button.
    [SerializeField] float curTimer;                        //The float that will be increasing when holding button.
    [SerializeField] float incremental = 0.2f;              //The amount that will increase over time.
    bool isHolding = false;

    [Space(20)]

    [Header("ADDITIONAL FLARES")]
    [SerializeField] bool useProgressBar;                   //Bool to check wether or not using the incrementFill image to indicate the holding progress.
    [SerializeField] Image incrementFill;                   //Ref to the progress image.

    [Space(20)]

    public UnityEvent holdFinish;                           //Event that will invoke when finish holding.
    public UnityEvent holdRelease;                          //Event that will invoke when finish holding.
    #endregion

    //dialogue for this specific class
    public Dialogue dialogue;
    public TextMeshPro nameText;
    
    public DialogueManager dialogueManager;
    [SerializeField] public GameObject textPopUp;

    public int npcIdNumber; //ID number

    public int ID { get; set; } //NPC ID for Quest identification
    public static event Action<NPCInterface> npcIDcheck;

    public NPCInterface instance; //instance NPCinterface
    public virtual void Start()
    {
        useProgressBar = true;
        //incrementFill = GameObject.Find("InteractionProgress").GetComponent<Image>();//init the variable
        instance = this; //refer the interface instance to this
        instance.ID = npcIdNumber; //Set this npc's ID
        //Get the dialogue manager
        dialogueManager = FindObjectOfType<DialogueManager>();
        textPopUp.SetActive(false);
        nameText.text = dialogue.speaker;
    }

    //If player is in the trigger of the character, check for initiate dialogue input
    private void OnTriggerEnter(Collider collider)
    {
        
        //Player is in the talking range, make the popup appears and enable the player to start the dialogue
        if (collider.gameObject.tag == "Player")
        {
            
            textPopUp.SetActive(true);
            //start dialogue if player is not in another dialogue and has pressed E
            if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.inDialogue)
            {

                //Set bool to true.
                isHolding = true;

                //Init the coroutine HoldingButton().
                StartCoroutine(HoldingButton());

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(curTimer >= endTimer)
            {
                npcIDcheck?.Invoke(instance); //Trigger event

                InitiateDialogue();

                ResetTimer();
            }
            

            //start dialogue if player is not in another dialogue and has pressed E
            if (Input.GetKey(KeyCode.E) && !dialogueManager.inDialogue)
            {
                //Set bool to true.
                isHolding = true;

                //Init the coroutine HoldingButton().
                StartCoroutine(HoldingButton());
            }

            if (Input.GetKeyUp(KeyCode.E) && !dialogueManager.inDialogue)
            {
                ResetTimer();

                holdRelease?.Invoke();

            }

            if(!dialogueManager.inDialogue) textPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //player is no longer in the talking range, disable the popup and do not let the player start conversation
        if(other.gameObject.tag =="Player")
        {
            ResetTimer();

            holdRelease?.Invoke();

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

    #region Backend Progress Bar
    /// <summary>
    /// When holding down button, the timer will increase by incremental value (set in the editor) over time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator HoldingButton()
    {
        //While loop to keep running if curTimer is smaller than endTimer.
        while (curTimer < endTimer)
        {
            //Break out of the loop if the mouse input is up.
            if (!isHolding) break;

            //Increase the curTimer value.
            curTimer += incremental * Time.deltaTime;

            if (useProgressBar) incrementFill.fillAmount = curTimer / endTimer;

            //Invoke the UnityEvent when the holding timer reached the end.
            if (curTimer >= endTimer)
            {
                curTimer = endTimer;
                holdFinish?.Invoke();
            }

            yield return null;
        }

        //Set the incrementFill value.
        if (useProgressBar) incrementFill.fillAmount = curTimer / endTimer;
    }

    //Reset the button to the start position.
    void ResetTimer()
    {
        isHolding = false;
        curTimer = startTimer;
        incrementFill.fillAmount = curTimer;
    }
    #endregion
}
