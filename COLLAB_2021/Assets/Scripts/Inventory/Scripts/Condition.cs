using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    //Public ConditionID enum property of the condition.
    public ConditionID Id { get; set; }

    //Public Name property of the condition.
    public string Name { get; set; }

    //Declare string property explain the condition.
    public string Description { get; set; }

    //Declare string property that display the message when the creature inflict with the status.
    public string StartMessage { get; set; }

    //Note: Can only assign a function that not return a value in Action.
    //
    public Action<Character> OnStart { get; set; }

    //An action takes Creature class as a parameter.
    //Call this also call Creature class.
    //Do a check call before perform a move.
    public Func<Character, bool> OnBeforeMove { get; set; }

    //An action takes Creature class as a parameter.
    //Call this also call Creature class.
    //Do a check call after a turn.
    public Action<Character> OnAfterTurn { get; set; }
}
