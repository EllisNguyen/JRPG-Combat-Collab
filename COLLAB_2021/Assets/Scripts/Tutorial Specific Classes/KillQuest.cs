using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Author: Ly Duong Huy
 * Class: KillQuest
 * Date: 27/04/2022
 * Summary: Goal for Killing Quest
 */

public class KillQuest : QuestGoal
{
    public int enemyID; //enemyID used to be compared with
    //the type that the quest will call in order to add into the goals list
    public KillQuest(QuestSystem quest, int monsterID, string description, bool completed, int currentAmount, int targetAmount)
    {
        this.quest = quest;
        this.enemyID = monsterID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = targetAmount;
    }

    public override void Initiate()
    {
        base.Initiate();
        EnemyMovement.enemyDead += EnemyDied; //subscribe to the enemy died event
    }

    //Check for enemy ID and increase the current amount of enemy type killed
    void EnemyDied(EnemyInterface enemyInterface)
    {
        if (enemyInterface.ID == this.enemyID)
        {
            this.currentAmount++;
            //double check to fix bug
            CheckRequirements();
            CheckRequirements();
        }
    }
}
