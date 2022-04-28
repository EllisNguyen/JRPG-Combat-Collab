using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetNPCQuest : QuestGoal
{
    public int npcID;
    public MeetNPCQuest(QuestSystem quest, int npcID, string description, bool completed, int currentAmount, int targetAmount)
    {
        this.quest = quest;
        this.npcID = npcID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = targetAmount;
    }

    public override void Initiate()
    {
        base.Initiate();
        DialogueCharacter.npcIDcheck += TalkedToNPC;
    }

    void TalkedToNPC(NPCInterface npcInterface)
    {
        if (npcInterface.ID == this.npcID)
        {
            this.currentAmount++;
            CheckRequirements();
        }
    }
}
