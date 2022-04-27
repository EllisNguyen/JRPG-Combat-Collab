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
    public int enemyID;
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
        EnemyMovement.enemyDead += EnemyDied;
    }

    void EnemyDied(EnemyInterface enemyInterface)
    {
        if (enemyInterface.ID == this.enemyID)
        {
            this.currentAmount++;
            CheckRequirements();
        }
    }
}
