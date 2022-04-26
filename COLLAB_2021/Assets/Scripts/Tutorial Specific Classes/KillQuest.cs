using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuest : QuestVariable
{
    public int enemyID;
    public KillQuest(int monsterID, string description, string title, bool completed, int currentAmount, int targetAmount)
    {
        this.enemyID = monsterID;
        this.description = description;
        this.title = title;
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
