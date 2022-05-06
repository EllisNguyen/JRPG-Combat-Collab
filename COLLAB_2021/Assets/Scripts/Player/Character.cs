///Author: Phab Nguyen
///Description: Stat storage and calculation.
///Day created: 11/11/2021
///Last edited: 28/03/2022 - Phab Nguyen.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Character
{

    [SerializeField] CharacterBaseStats _base;
    [SerializeField] int level;

    public List<Move> Moves { get; set; }

    Move _currentMove;

    //Store current move.
    public Move CurrentMove
    {
        get { return _currentMove; }
        set { _currentMove = value; }
    }

    //Constructor.
    //Create a new instance of the character.
    public Character(CharacterBaseStats cBase, int pLevel)
    {
        _base = cBase;
        level = pLevel;

        Init();
    }

    //Reference to CharacterBaseStats script.
    public CharacterBaseStats Base
    {
        get { return _base; }
        set { _base = value; }
    }

    //Property that store the current status condition.
    public Condition Status { get; private set; }

    //Declare the number of turn that the status effect will last.
    public int StatusTime { get; set; }

    //Store the status that only last during the battle.
    public Condition VolatileStatus { get; private set; }

    //Declare the number of turn that the Volatile status effect will last.
    public int VolatileStatusTime { get; set; }

    //List of element:
    //Queue of string to display what happened when apply status effect.
    //Init the Queue before adding anything to it.
    public Queue<string> StatusChanges { get; private set; }

    public int Level { get { return level; } }

    public int Exp { get; set; }

    public int HP { get; set; }
    public int MP { get; set; }

    //Event to call when inflict status condition.
    //Update Icon and UI.
    public event System.Action OnStatusChanged;

    //Event to call when HP changed.
    //Update every other UI.
    public event System.Action OnHPChanged;
    public event System.Action OnMPChanged;

    //Initialize character's stats.
    //Add move according to learnable moves list.
    //Calculate or modify the stats once.
    public void Init()
    {
        Moves = new List<Move>();

        foreach (var move in _base.LearnableSkills)
        {
            if (move.Level <= level) Moves.Add(new Move(move.Base));
        }

        //Set stats
        CalculateStats();
        HP = MaxHP;
        MP = MaxMP;

        //Initialize the status changes.
        StatusChanges = new Queue<string>();

        //Reset all boosted stats.
        ResetStatBoost();
        Status = null;
        VolatileStatus = null;
    }

    //Dictionary that store all character's stats key.
    //Privately set the stats only.
    public Dictionary<Stat, int> Stats { get; private set; }

    //Extended dictionary to store all the boosted value of the stats.
    public Dictionary<Stat, int> StatBoosts { get; private set; }

    int GetStat(Stat stat)
    {
        //Get prefix stats value.
        int statVal = Stats[stat];

        //Apply stats boosting effect.
        //Get the boosted value of the stat from StatBoost the Dictionary.
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };//An array of value that boosting the stat.

        //If the boost is >= 0, then it's positive boosting.
        //Multiply the current stat with the boostValue.
        if (boost >= 0) { statVal = Mathf.FloorToInt(statVal * boostValues[boost]); }
        //If the boost is < 0, then it's negative boosting.
        //Divide the current stat with the boostValue.
        else { statVal = Mathf.FloorToInt(statVal / boostValues[-boost]); }

        return statVal;
    }

    void CalculateStats()
    {
        //Init stats dictionary.
        Stats = new Dictionary<Stat, int>();

        //Calculate and store value of each stat:
        /*ATK*/
        Stats.Add(Stat.physicalAtkDmg, Mathf.FloorToInt((Base.physicalAtkDmg * Level) / 100f) + 5);
        
        /*DEF*/
        Stats.Add(Stat.physicalDef, Mathf.FloorToInt((Base.physicalDef * Level) / 100f) + 5);
        
        /*SPATK*/
        Stats.Add(Stat.specialAtkDmg, Mathf.FloorToInt((Base.specialAtkDmg * Level) / 100f) + 5);
       
        /*SPDEF*/
        Stats.Add(Stat.specialDef, Mathf.FloorToInt((Base.specialDef * Level) / 100f) + 5);
        
        /*SPD*/
        Stats.Add(Stat.speed, Mathf.FloorToInt((Base.speed * Level) / 100f) + 5);

        //Calculate HP
        MaxHP = Mathf.FloorToInt((Base.health * Level) / 100f) + 10 + Level;

        //Calculate HP
        MaxMP = Mathf.FloorToInt((Base.mana * Level) / 100f) + 5 + Level;
    }

    //Modify stat boost dictionary when a status move is performed.
    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        //Loop through all listed boost that set in the editor.
        foreach (var statBoost in statBoosts)
        {
            //Store value of stat and boost into variables.
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            //Add the boost(s) into the stat(s) in the stat boost dictionary.
            //Finalize the value and clamp the value between -6 and 6.
            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            //Show dialogue.
            if (boost > 0)
            {
                //Add a dialogue to the queue to run.
                StatusChanges.Enqueue(($"{Base.charName}'s {stat} rose !!"));
            }
            else
            {
                //Add a dialogue to the queue to run.
                StatusChanges.Enqueue(($"{Base.charName}'s {stat} fell !!"));
            }

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}.");
        }
    }

    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Reset all the stats.
    /// Call when end battle or when new battle start.
    /// </summary>
    void ResetStatBoost()
    {
        //Set all stats enum key back to value 0.
        StatBoosts = new Dictionary<Stat, int>()
        {
            //Set value to 0 when none of the stats are boosted.
            {Stat.physicalAtkDmg, 0},
            {Stat.physicalDef, 0},
            {Stat.specialAtkDmg, 0},
            {Stat.specialDef, 0},
            {Stat.speed, 0},
            {Stat.critChance, 0},
            {Stat.critDmg, 0},
        };
    }

    #region Stats
    /// <summary>
    /// STAT: ATTACK
    /// Return the property value calculated by GetStat().
    /// </summary>
    public int PhysATK
    {
        //Formula from pokemon
        get { return GetStat(Stat.physicalAtkDmg); }
    }

    /// <summary>
    /// STAT: DEFENSE
    /// Return the property value calculated by GetStat().
    /// </summary>
    public int PhysDEF
    {
        //Formula from pokemon
        get { return GetStat(Stat.physicalDef); }
    }

    /// <summary>
    /// STAT: SPECIAL ATTACK
    /// Return the property value calculated by GetStat().
    /// </summary>
    public int SpecATK
    {
        //Formula from pokemon
        get { return GetStat(Stat.specialAtkDmg); }
    }

    /// <summary>
    /// STAT: SPECIAL DEFENSE
    /// Return the property value calculated by GetStat().
    /// </summary>
    public int SpecDEF
    {
        //Formula from pokemon
        get { return GetStat(Stat.specialDef); }
    }

    /// <summary>
    /// STAT: SPEED
    /// Return the property value calculated by GetStat().
    /// </summary>
    public int Speed
    {
        //Formula from pokemon
        get { return GetStat(Stat.speed); }
    }

    /// <summary>
    /// STAT: HEALTH
    /// </summary>
    public int MaxHP { get; private set; }
    public int MaxMP { get; private set; }

    #endregion

    //Formula follow Pokemon.
    //Damage calculation: bulbapedia.bulbagarden.net/wiki/Damage
    public DamageDetails TakeDamage(Move move, Character attacker)
    {
        //Crit hit calculation in pokemon
        float critical = 1f;
        if (UnityEngine.Random.value * 100f <= 6.25f)
        {
            critical = 1.5f;
        }

        //Get types of the creature receiving damage.
        float element = ElementChart.ElementalModifier(move.Base.Element, attacker.Base.element);
        Debug.Log(element);

        //
        var damageDetails = new DamageDetails()
        {
            ElementalModifier = element,
            Critical = critical,
            Fainted = false
        };

        //Conditional operator:
        //Chose which calculation formula base on attaker move (spATK or ATK).
        float attack = (move.Base.Category == MoveCategory.Range) ? attacker.SpecATK : attacker.PhysATK;
        float defense = (move.Base.Category == MoveCategory.Range) ? attacker.SpecDEF : attacker.PhysDEF;

        //Vary the receiving damage (but minimal).
        //Multiply with the type effectiveness and critical randomness.
        float modifier = UnityEngine.Random.Range(0.85f, 1f) * element * critical;

        //
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifier);

        //Receive damage, check if creature is fainted.
        DecreaseHP(damage);

        return damageDetails;
    }

    #region HP control
    public void DecreaseHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        OnHPChanged?.Invoke();
    }

    public void IncreaseHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHP);
        OnHPChanged?.Invoke();
    }
    #endregion HP control

    #region MP control
    public void DecreaseMP(int mana)
    {
        MP = Mathf.Clamp(MP - mana, 0, MaxMP);
        OnMPChanged?.Invoke();
    }

    public void IncreaseMP(int amount)
    {
        MP = Mathf.Clamp(MP + amount, 0, MaxMP);
        OnMPChanged?.Invoke();
    }
    #endregion MP control

    //Apply status condition to the creature.
    public void SetStatus(ConditionID conditionId)
    {
        if (Status != null) return;

        //Get condition ID from ConditionDB dictionary.
        //Store it to Status property.
        Status = ConditionsDB.Conditions[conditionId];

        //Check if a status is applied and require OnStart Action.
        Status?.OnStart?.Invoke(this);

        //Add the status dialogue to queue.
        StatusChanges.Enqueue($"{Base.charName} {Status.StartMessage}");

        //Apply status condition indicator.
        OnStatusChanged?.Invoke();
    }

    //Kill the status condition on the creature.
    public void CureStatus()
    {
        Status = null;

        //Reset status condition indicator.
        OnStatusChanged?.Invoke();
    }

    //Apply status condition to the creature.
    public void SetVolatileStatus(ConditionID conditionId)
    {
        if (VolatileStatus != null) return;

        //Get condition ID from ConditionDB dictionary.
        //Store it to Status property.
        VolatileStatus = ConditionsDB.Conditions[conditionId];

        //Check if a status is applied and require OnStart Action.
        VolatileStatus?.OnStart?.Invoke(this);

        //Add the status dialogue to queue.
        StatusChanges.Enqueue($"{Base.charName} {VolatileStatus.StartMessage}");
    }

    //Kill the status condition on the creature.
    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }

    //For AI enemy.
    //Choose a random move out of its selected move.
    public Move GetRandomMove()
    {
        //Declare a list of move with PP available.
        var movesWithMana = Moves.Where(x => x.Mana > 0).ToList();

        //Run the move that have PP.
        int r = UnityEngine.Random.Range(0, movesWithMana.Count);
        return movesWithMana[r];
    }

    //A boolean check.
    public bool OnBeforeMove()
    {
        //Boolean to check wether creature can perform a move.
        bool canPerformMove = true;

        //Check if status condition is not null.
        if (Status?.OnBeforeMove != null)
        {
            if (!Status.OnBeforeMove(this))
                canPerformMove = false;
        }

        //Check if volatile status is not null.
        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (!VolatileStatus.OnBeforeMove(this))
                canPerformMove = false;
        }

        return canPerformMove;
    }

    //This func fire when a turn ended.
    public void OnAfterTurn()
    {
        //Null condition operator.
        //Call the action if status is not null.
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }

    public static event Action<CharacterBaseStats> enemyDead;//Event to for quest check

    //This func fire when battle is over.
    public void OnBattleOver()
    {
        Debug.Log("Name: " + _base.charName);
        enemyDead?.Invoke(_base);
        VolatileStatus = null;
        ResetStatBoost();
    }

    
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float ElementalModifier { get; set; }
}

[System.Serializable]
public class CharacterSaveData
{
    public string name;//Use this to get all data of the creature in the scriptable object.

    public int hp;
    public int level;
    public int exp;
    public ConditionID? statusId;
    public List<MoveSaveData> moves;
}