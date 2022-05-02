using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;

/*Author: Ly Duong Huy
 *Class: MeetGoal
 *Date: 27/04/2022
 *Summary: A Goal for an NPC Meeting Goal
 *Note: Quest requirement amount should always be 1 as the player just need to speak to that NPC once
 */

public class MeetNPC : Quest.QuestGoal
{
    public string npcName; //Name of a monster to fill in the description
    [SerializeField] int npcID; //ID of the monster

    public override string GetDescription()
    {
        return $"Meet {npcName}";
        //$ is short-hand for String.Format and is used with string interpolations
        //More info: https://stackoverflow.com/questions/31014869/what-does-mean-before-a-string
    }
    public override void Initilize()
    {
        base.Initilize();
        //Call OnNPCMet when interact with an NPC
        DialogueCharacter.npcIDcheck += OnNPCMet;
    }

    //Check if the NPC interacted is the NPC required in the Quest
    private void OnNPCMet(NPCInterface npcInterface)
    {
        //if the NPC's ID is the same then add the current amount
        if (npcInterface.ID == this.npcID) 
            CurrentAmount++;
        Evaluate();
    }
}
