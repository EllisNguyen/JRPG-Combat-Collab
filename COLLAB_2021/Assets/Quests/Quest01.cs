using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Author: Ly Duong Huy
 *Class: Quest01
 *Date: 27/04/2022
 *Summary: Store Quest 01 data
 */
public class Quest01 : QuestSystem
{
    

    private void Start()
    {

        //Quest Info
        questName = "Tutorial: Movements";
        description = "Learn the basics";
        questID = 0;
        experienceReward = 100;

        Goals.Add(new MeetNPCQuest(this,1,"Read the Bulletin",false,0,1));
        

        //Initiate the goals
        Goals.ForEach(g => g.Initiate());
    }

}
