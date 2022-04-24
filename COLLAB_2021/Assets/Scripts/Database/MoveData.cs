using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Creature/Create new move")]
public class MoveData : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] elements type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] bool alwaysHit;
    [SerializeField] int pp;
    [SerializeField] [Range(-7, 5)] int priority;
    [SerializeField] MoveCategory category;
    [SerializeField] MoveEffect effect;
    [SerializeField] List<SecondaryEffects> secondaries;
    [SerializeField] MoveTarget target;

    #region getter for name and description
    //Expose name variable using property
    public string Name
    {
        //Get the value of the property
        get { return name; }
    }

    //Expose description variable using property
    public string Description
    {
        //Get the value of the property
        get { return description; }
    }
    #endregion

    //Expose name variable using property
    public elements Type
    {
        //Get the value of the property
        get { return type; }
    }

    #region Move's stats
    //Expose name variable using property
    public int Power
    {
        //Get the value of the property
        get { return power; }
    }

    //Expose name variable using property
    public int Accuracy
    {
        //Get the value of the property
        get { return accuracy; }
    }

    //Expose name variable using property
    public bool AlwaysHit
    {
        //Get the value of the property
        get { return alwaysHit; }
    }

    //Expose PP int variable using property
    public int PP
    {
        //Get the value of the property
        get { return pp; }
    }

    //Expose Priority int variable using property
    public int Priority
    {
        //Get the value of the property
        get { return priority; }
    }

    //Expose name variable using property
    public MoveCategory Category
    {
        //Get the value of the property
        get { return category; }
    }

    //Expose MoveEffect class using property
    public MoveEffect Effect
    {
        //Get the value of the property
        get { return effect; }
    }

    //Expose SecondaryEffects class using property
    public List<SecondaryEffects> Secondaries
    {
        //Get the value of the property
        get { return secondaries; }
    }

    //Expose MoveTarget class using property
    public MoveTarget Target
    {
        //Get the value of the property
        get { return target; }
    }
    #endregion
}

public enum MoveCategory
{
    Physical,
    Special,
    Status
}

[System.Serializable]
public class MoveEffect
{
    //Serialize the list
    [SerializeField] List<StatBoost> boosts;

    //Declare ConditionID enum.
    [SerializeField] ConditionID status;

    //Declare volatile status conditions.
    [SerializeField] ConditionID volatileStatus;

    //Expose the list of ^^^ stat boost.
    public List<StatBoost> Boost
    {
        get { return boosts; }
    }

    //Expose the condition status list.
    public ConditionID Status
    {
        get { return status; }
    }

    //Expose the volatile status list.
    public ConditionID VolatileStatus
    {
        get { return volatileStatus; }
    }
}

[System.Serializable]
public class SecondaryEffects : MoveEffect
{
    [Range(0,100)]
    [SerializeField] int chance;
    [SerializeField] MoveTarget target;

    //Expose the chance int.
    public int Chance
    {
        get { return chance; }
    }

    //Expose the target property list.
    public MoveTarget Target
    {
        get { return target; }
    }
}

/// <summary>
/// This class will act as an exposed key and value of the stats,
/// so the MoveEffect class can serialize the list of stat effect.
/// </summary>
[System.Serializable]
public class StatBoost
{
    public Stat stat;
    [Range(-6, 6)] public int boost;
}

public enum MoveTarget
{
    Foe,Self
}