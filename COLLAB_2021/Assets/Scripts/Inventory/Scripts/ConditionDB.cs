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
        {
            //Define the POISON status conditions.
            ConditionID.rad,
            new Condition
            {
                Name = "Radiated",
                StartMessage = "has been poisoned by radiation.",
                //A lambda function
                OnAfterTurn = (Character creature) =>
                {
                    creature.DecreaseHP(creature.MaxHP / 8);
                    creature.StatusChanges.Enqueue($"{creature.Base.charName.ToUpper()} hurt by the radiation.");
                }
            }
        },
        {
            //Define the BURN status conditions.
            ConditionID.brn,
            new Condition
            {
                Name = "Burn",
                StartMessage = "has been burned.",
                //A lambda function
                OnStart = (Character character) =>
                {
                    Dictionary<Stat, float> StatBoosts;

                    StatBoosts = new Dictionary<Stat, float>()
                    {
                        {Stat.SpecDEF, -GetStatusBonus(character.Status)},
                    };

                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} Phys.DEF fell.");
                },
                OnAfterTurn = (Character character) =>
                {
                    character.DecreaseHP(character.MaxHP / 16);
                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} hurt by burn.");
                }
            }
        },
        {
            //Define the PARALYZE status conditions.
            ConditionID.par,
            new Condition
            {
                Name = "Paralyze",
                StartMessage = "has been paralyzed.",
                //A lambda function
                OnStart = (Character character) =>
                {
                    Dictionary<Stat, float> StatBoosts;

                    StatBoosts = new Dictionary<Stat, float>()
                    {
                        {Stat.SpecDEF, -GetStatusBonus(character.Status)},
                    };

                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} Phys.ATK fell.");
                },
                OnBeforeMove = (Character character) =>
                {
                    //Randomly roll a number if the creature can perform a move.
                    if(Random.Range(1, 5) == 1)
                    {
                        character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} paralyzed and cannot move.");
                        return false;
                    }
                    return true;
                }
            }
        },

        {
            //Define the DECAY status conditions.
            ConditionID.dec,
            new Condition
            {
                Name = "Decay",
                StartMessage = "turned into a walking corpse.",
                //A lambda function
                OnStart = (Character character) =>
                {
                    Dictionary<Stat, float> StatBoosts;

                    StatBoosts = new Dictionary<Stat, float>()
                    {
                        {Stat.SpecDEF, -GetStatusBonus(character.Status)},
                    };

                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} Spec.DEF fell.");
                },
                OnBeforeMove = (Character character) =>
                {
                    //Randomly roll a number if the creature can perform a move.
                    if(Random.Range(1, 3) == 1)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} is healthy again!");
                        return true;
                    }

                    character.ResetStatBoost();
                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} is struggling.");
                    return false;
                },
            }
        },
        {
            //Define the BLIND status conditions.
            ConditionID.bln,
            new Condition
            {
                Name = "Blind",
                StartMessage = "cannot see anything.",
                OnStart = (Character character) =>
                {
                    //Sleep for 1-3 turn(s).
                    character.StatusTime = Random.Range(1, 3);
                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} will be blinded for {character.StatusTime} turn(s).");
                },
                //A lambda function
                OnBeforeMove = (Character character) =>
                {
                    //Check if status timer reached 0.
                    if(character.StatusTime <= 0)
                    {
                        //Cure the status, set to null.
                        character.CureStatus();

                        //Set wake up dialogue.
                        character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} eyes are wide opened.");
                        return true;
                    }

                    //Decrease the set status timer.
                    character.StatusTime--;

                    //Set sleeping dialogue.
                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} still can't see shit.");

                    return false;
                }
            }
        },

        //Volatile status condition.
        {
            //Define the CONFUSION status conditions.
            ConditionID.confusion,
            new Condition
            {
                Name = "Confusion",
                StartMessage = "is confused.",
                OnStart = (Character character) =>
                {
                    //Confused for 1-4 turn(s).
                    character.VolatileStatusTime = Random.Range(1, 4);
                    Debug.Log($"Will be confused for {character.VolatileStatusTime} moves.");
                },
                //A lambda function
                OnBeforeMove = (Character character) =>
                {
                    //Check if status timer reached 0.
                    if(character.VolatileStatusTime <= 0)
                    {
                        //Cure the status, set to null.
                        character.CureVolatileStatus();

                        //Set wake up dialogue.
                        character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} snapped out of confusion.");
                        return true;
                    }

                    //Decrease the set status timer.
                    character.VolatileStatusTime--;

                    //50% chance to perform a move.
                    if(Random.Range(1, 3) == 1)
                        return true;

                    //Set confusion dialogue.
                    //Hurt the creature.
                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} is confused.");
                    character.DecreaseHP(character.MaxHP / 8);
                    character.StatusChanges.Enqueue($"{character.Base.charName.ToUpper()} hurt itself in its confusion.");

                    return false;
                }
            }
        },
    };

    //Status bonus apply for catch rate.
    public static float GetStatusBonus(Condition condition)
    {
        if (condition == null)
            return 1f;
        else if (condition.Id == ConditionID.bln || condition.Id == ConditionID.dec)
            return 2f;
        else if (condition.Id == ConditionID.par || condition.Id == ConditionID.rad || condition.Id == ConditionID.brn)
            return 1.5f;

        return 1f;
    }
}

public enum ConditionID
{
    none,
    rad,            //RADIATED
    brn,            //BURNT
    par,            //PARALYZE
    bln,            //BLINDED
    dec,            //DECAY
    confusion,      //CONFUSION
}