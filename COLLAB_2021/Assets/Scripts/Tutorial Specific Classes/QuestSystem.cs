using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Query Library
using System;

/*Author: Ly Duong Huy
 *Class: QuestSystem
 *Date: 27/04/2022
 *Summary: Handles the QuestSystem
 */

public class QuestSystem : MonoBehaviour
{
    //Variables
    //Making a list of quests
    public List<QuestGoal> Goals = new List<QuestGoal>();

    //Info
    public string questName;
    public int questID;
    [TextArea(3,10)]
    public string description;
    public int experienceReward;
    public bool completed;
    

    public void CheckGoals()
    {
        //Check if all of the goals are completed (using lamba expression)
        completed = Goals.All(g => g.completed);
        
        //If the goals are completed then quest is completed. Give Rewards
        //if (completed) GiveReward();
    }

    public void GiveReward()
    {
        
    }
}
