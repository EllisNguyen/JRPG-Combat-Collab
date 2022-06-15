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

    [SerializeField] List<GameObject> questNPC2 = new List<GameObject>();
    [SerializeField] List<GameObject> questNPC2Enemy = new List<GameObject>(); //reference to the 2nd Quest Giver
    //[SerializeField] GameObject menuTransitionObject; //reference to the game object that loads menu

    // Start is called before the first frame update
    void Start()
    {

        //menuTransitionObject = GameObject.Find("ToMenu");

        //menuTransitionObject.SetActive(false); //Disable ToMenu Load object until quest 2 completion
        foreach (GameObject item in questNPC2)
        {
            item.SetActive(false);
        } //Disable Quest Giver 2 until Quest 1 is completed

        foreach (GameObject item in questNPC2Enemy)
        {
            item.SetActive(false);
        }
        QuestNPC.questCompleted += InitQuest1;
        QuestNPC.questAssigned += InitQuest2;
    }

    private void InitQuest2(Quest quest02)
    {
        if (quest02.name == "QuestTut02")
        {
            foreach (GameObject item in questNPC2Enemy)
            {
                item.SetActive(true);
            }
            //menuTransitionObject.SetActive(true);
        }
    }

    //Called in the event to enable Quest Giver 02
    void InitQuest1(Quest quest01)
    {

        if (quest01.name == "QuestTut01")
        {

            foreach (GameObject item in questNPC2)
            {
                item.SetActive(true);
            } //Enable Quest Giver 2 after Quest 1 is completed
        }
    }
}
