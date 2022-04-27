using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Author: Ly Duong Huy
 * Date: 26/04/2022
 * Class: QuestVariable
 * Obj:
 * Summary: The basic variables for quest
 */

[System.Serializable]
public class QuestVariable
{
    public bool completed;

    //Variables
    public string title;
    [TextArea(3, 10)]
    public string description;
    public int currentAmount;
    public int requiredAmount;

    public virtual void Initiate()
    {
        
    }

    public void CheckRequirements()
    {
        if (currentAmount >= requiredAmount) Complete();
    }

    public void Complete()
    {
        completed = true;
    }
}