using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Author: Ly Duong Huy
 * Date: 02/06/2022
 * Class: DirectorC1
 * Summary: Controls the flow of the chapter 01
 */
public class DirectorC1 : MonoBehaviour
{
    //Variables

    [SerializeField] List<GameObject> quest13 = new List<GameObject>(); //ref to the stuffs for quest 1-3
    [SerializeField] List<GameObject> quest14 = new List<GameObject>(); //ref to stuffs quest 1-4



    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in quest13)
        {
            item.SetActive(false);
        } //Disable the quest stuffs in quest 1-3
        QuestNPC.questAssigned += InitQuest1;
        QuestNPC.questAssigned += InitQuest2;

    }


    private void InitQuest2(Quest quest02)
    {
        if (quest02.name == "Quest 1-4")
        {
            foreach (GameObject item in quest14)
            {
                item.SetActive(true);
            } //Enable Quest monster 1-4 after Quest 1-4 is assigned
        }
    }

    //Called in the event to enable Quest Giver 02
    void InitQuest1(Quest quest01)
    {

        if (quest01.name == "Quest 1-3")
        {

            foreach (GameObject item in quest13)
            {
                item.SetActive(true);
            } //Enable Quest 1-3 monsters after Quest 1-3 is assigned
        }
    }

}
