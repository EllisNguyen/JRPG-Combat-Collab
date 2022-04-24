///Author: Phab Nguyen
///Description: Stat storage and calculation.
///Day created: 11/11/2021
///Last edited: 28/03/2022 - Phab Nguyen.

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{

    [SerializeField] CharacterBaseStats _base;
    [SerializeField] int level;

    //Constructor.
    //Create a new instance of the character.
    public Character(CharacterBaseStats cBase, int pLevel)
    {
        _base = cBase;
        level = pLevel;

        Init();
    }

    //Reference to CharacterBaseStats script.
    public CharacterBaseStats Base { get { return _base; } }
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
        //Set stats
        CalculateStats();
        HP = MaxHP;
        MP = MaxMP;
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
        //An array of value that boosting the stat.
        //The boosted stat will multiply by the value in the float array.
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

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
    public void DecreaseMP(int damage)
    {
        MP = Mathf.Clamp(MP - damage, 0, MaxMP);
        OnMPChanged?.Invoke();
    }

    public void IncreaseMP(int amount)
    {
        MP = Mathf.Clamp(MP + amount, 0, MaxMP);
        OnMPChanged?.Invoke();
    }
    #endregion MP control
}
