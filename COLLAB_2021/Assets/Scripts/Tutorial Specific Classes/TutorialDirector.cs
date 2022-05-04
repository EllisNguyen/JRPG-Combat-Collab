using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Author: Ly Duong Huy
 * Date: 04/05/2022
 * Class: TutorialDirector
 * Summary: Controls the flow of the tutorial
 */

public class TutorialDirector : MonoBehaviour
{
    //Variables

    [SerializeField] GameObject questNPC2; //reference to the 2nd Quest Giver
    [SerializeField] GameObject menuTransitionObject; //reference to the game object that loads menu

    // Start is called before the first frame update
    void Start()
    {

        menuTransitionObject = GameObject.Find("ToMenu");
        questNPC2 = GameObject.Find("Andre");
        menuTransitionObject.SetActive(false); //Disable ToMenu Load object until quest 2 completion
        questNPC2.SetActive(false); //Disable Quest Giver 2 until Quest 1 is completed
        QuestNPC.questAssigned += InitQuest1;
        QuestNPC.questAssigned += InitQuest2;
    }

    private void InitQuest2(Quest quest02)
    {
        if(quest02.name == "QuestTut02")
        {
            menuTransitionObject.SetActive(true);
        }
    }

    //Called in the event to enable Quest Giver 02
    void InitQuest1(Quest quest01)
    {
        
        if (quest01.name == "QuestTut01")
        {
            
            questNPC2.SetActive(true);
        }
    }
}
