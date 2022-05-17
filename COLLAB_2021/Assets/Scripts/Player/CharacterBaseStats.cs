///Author: Quan.TM
///Description: Hold the stats for characters and attributes.
///Day created: 02/11/2021
///Last edited: 28/04/2021 - Phab Nguyen.

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterBaseStats : ScriptableObject
{
    #region PlayerStats
    [Header("Character Visual")]
    public string charName;
    [PreviewField] public Sprite portraitSprite;
    [PreviewField] public Sprite battleSprite;
    [PreviewField] public Sprite attackSprite;
    [PreviewField] public Sprite guardSprite;
    [PreviewField] public Sprite battleIcon;
    public List<Sprite> overworldAnim;

    [Header("Character Stats")]
    [TextArea] public string charDescription;
    [Range(1,100)] public int health;
    [Range(1,100)] public int mana;
    [Range(1,100)] public int physicalAtkDmg;
    [Range(1,100)] public int specialAtkDmg;
    [Range(1,100)] public int physicalDef;
    [Range(1,100)] public int specialDef;
    [MinMaxSlider(1, 100, true)] public Vector2 speed = new Vector2(20, 20);
    [Range(1,70)] public int luck;
    [Range(1,100)] public int elementalRes;
    [Range(1,100)] public int expYield;
    public elements element; //Reference to the elements enum
    #endregion

    public int MinSpeed => Mathf.FloorToInt(speed.x);
    public int MaxSpeed => Mathf.FloorToInt(speed.y);

    [SerializeField] GrowthRate growthRate;
    [SerializeField] List<LearnableSkill> learnableSkills;

    public List<LearnableSkill> LearnableSkills { get { return learnableSkills; } }

    public void OnValidate()
    {
        charName = name;
    }

    //Growth rate formula, the required EXP for each level.
    public int GetExpForLevel(int level)
    {
        if (growthRate == GrowthRate.Fast)
        {
            return 4 * (level * level * level) / 5;
        }
        else if (growthRate == GrowthRate.MediumFast)
        {
            return (level * level * level);
        }
        else if (growthRate == GrowthRate.MediumSlow)
        {
            return 6 * (level * level * level) / 5 - 15 * (level * level) + 100 * level - 140;
        }
        else if (growthRate == GrowthRate.Slow)
        {
            return 5 * (level * level * level) / 4;
        }
        else if (growthRate == GrowthRate.Fluctuating)
        {
            return GetFluctuating(level);
        }
        return -1;
    }

    // Created a new method for getting the fluctuating levels
    public int GetFluctuating(int level)
    {
        if (level <= 15)
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * ((Mathf.Floor((level + 1) / 3) + 24) / 50));
        }
        else if (level >= 15 && level <= 36)
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * ((level + 14) / 50));
        }
        else
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * ((Mathf.Floor(level / 2) + 32) / 50));
        }
    }

    public void RandomAttributes()
    {
        health = Random.Range(5, 50);
        mana = Random.Range(5, 50);
        physicalAtkDmg = Random.Range(5, 50);
        specialAtkDmg = Random.Range(5, 50);
        physicalDef = Random.Range(5, 50);
        specialDef = Random.Range(5, 50);
        speed = new Vector2(Random.Range(5, 50), Random.Range(5, 50));
        luck = Random.Range(5, 50);
        elementalRes = Random.Range(5, 50);
    }
}

[System.Serializable]
public class LearnableSkill
{
    [SerializeField] MoveData skillData;
    [SerializeField] int level;

    public MoveData Base { get { return skillData; } }
    public int Level { get { return level; } }
}

public enum elements
{
    normal,
    heat,
    electric,
    radiation,
    ice,
    light,
    dark
};

public enum GrowthRate
{
    Fast,
    MediumFast,
    MediumSlow,
    Slow,
    Fluctuating
}
public class ElementChart //Type effectiveness
{
    static float[][] chart =    //Row = attacker; Col = defender
    {
            //                          Nor  Hea   Ele  Rad  Ice  Lgt   Dar     
            /*Normal*/      new float[]{1f,  1f,   1f,  1f,  1f,  1f, 1f},
            /*Heat*/        new float[]{1f,  1f,   1f,  1f,  2f,  0.5f, 1f},
            /*Electric*/    new float[]{1f,  1f, 1f,  2f,  0.5f,  0.5f,   1f},
            /*Radiation*/   new float[]{1f,  1f,   2f,  2f,  1f,  1f,   1f},
            /*Ice*/         new float[]{1f,  0.5f, 1f,  2f,  0f,  1f,   0.5f},
            /*Light*/       new float[]{1f,  2f,   1f,  1f,  1f,  0f,   2f},
            /*Dark*/        new float[]{1f,  1f,   1f,  1f,  2f,  2f,   0f},
        };

    public static float ElementalModifier(elements attacker, elements defender)
    {
        int row = (int)attacker;
        int col = (int)defender;

        return chart[row][col];
    }
}

//Stats enum for other script ref
public enum Stat
{
    health,
    mana,
    PhysATK,
    SpecATK,
    PhysDEF,
    SpecDEF,
    SPEED,
    Luck,

    Accuracy,
    Evasion
}