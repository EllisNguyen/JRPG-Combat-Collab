using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    //Initialize all status condition.
    public static void Init()
    {
        //Loop through all Condition Dictionary elements
        foreach (var kvp in Conditions)
        {
            //Store key and value into variables.
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            //Assign the ID in Condition Class here.
            condition.Id = conditionId;
        }
    }

    //Declare a dictionary that store all conditionID and condition's profile.
    //Init.
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {



    };

    //Status bonus apply for catch rate.
    public static float GetStatusBonus(Condition condition)
    {
        if (condition == null)
            return 1f;
        else if (condition.Id == ConditionID.slp || condition.Id == ConditionID.frz)
            return 2f;
        else if (condition.Id == ConditionID.par || condition.Id == ConditionID.psn || condition.Id == ConditionID.brn)
            return 1.5f;

        return 1f;
    }
}

public enum ConditionID
{
    none,
    psn,            //POISON
    brn,            //BURN
    slp,            //SLEEP
    par,            //PARALYZE
    frz,            //FROZEN
    confusion,      //CONFUSION
}