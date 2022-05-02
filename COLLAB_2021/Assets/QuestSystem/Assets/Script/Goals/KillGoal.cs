using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;

/*Author: Ly Duong Huy
 *Class: KillGoal
 *Date: 27/04/2022
 *Summary: A Goal for a Killing Monster Quest
 */

public class KillGoal : Quest.QuestGoal
{
    public string Monster; //Name of a monster to fill in the description
    [SerializeField] int monsterID; //ID of the monster

    public override string GetDescription()
    {
        return $"Kill {Monster}";
        //$ is short-hand for String.Format and is used with string interpolations
        //More info: https://stackoverflow.com/questions/31014869/what-does-mean-before-a-string
    }
    public override void Initilize()
    {
        base.Initilize();
        EnemyMovement.enemyDead += OnMonsterDie;
    }
    private void OnMonsterDie(EnemyInterface enemyInterface)
    {
        if (enemyInterface.ID == this.monsterID) 
            CurrentAmount++;
        Evaluate();
    }
}
