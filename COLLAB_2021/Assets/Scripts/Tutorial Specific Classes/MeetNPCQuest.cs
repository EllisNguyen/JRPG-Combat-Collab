using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Author: Ly Duong Huy
 * Class: MeetNPCQuest
 * Date: 27/04/2022
 * Summary: Goal for Meet a specific NPC quest
 */
public class MeetNPCQuest : QuestGoal
{
    public int npcID; //npcID which is used to be compare with
    //the type that the quest will call in order to add into the goals list
    public MeetNPCQuest(QuestSystem quest, int npcID, string description, bool completed, int currentAmount, int targetAmount)
    {
        this.quest = quest;
        this.npcID = npcID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = targetAmount;
    }
    //override the base Initiate
    public override void Initiate()
    {
        base.Initiate();
        //Listen to the npcIDcheck event each time talk with an NPC
        DialogueCharacter.npcIDcheck += TalkedToNPC;
    }
    /// <summary>
    /// Function called to check if the player talked to the correct NPC
    /// </summary>
    /// <param name="npcInterface"></param>
    void TalkedToNPC(NPCInterface npcInterface)
    {
        if (npcInterface.ID == this.npcID)
        {
            //if the ID is the same then we talked to the correct NPC increase the current amount to 1
            this.currentAmount++;

            //Double check to temporary fix a bug
            CheckRequirements();
            CheckRequirements();
        }
    }
}
