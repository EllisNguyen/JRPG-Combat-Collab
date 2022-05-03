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
    [PreviewField] public Sprite battleIcon;
    public List<Sprite> overworldAnim;

    [Header("Character Stats")]
    [TextArea] public string charDescription;
    [Range(5,100)] public int health = 50;
    [Range(5,100)] public int mana;
    [Range(5,100)] public int physicalAtkDmg;
    [Range(5,100)] public int specialAtkDmg;
    [Range(5,100)] public int physicalDef;
    [Range(5,100)] public int specialDef;
    [Range(5,100)] public int speed;
    [Range(5,100)] public int critChance;
    [Range(5,100)] public int critDmg;
    [Range(5,100)] public int elementalRes;
    [Range(5,100)] public int exp;
    public elements eles; //Reference to the elements enum
    #endregion

    [SerializeField] List<LearnableSkill> learnableSkills;

    public List<LearnableSkill> LearnableSkills { get { return learnableSkills; } }

    public void OnValidate()
    {
        charName = name;
    }

    [Button]
    public void RandomAttributes()
    {
        health = Random.Range(5, 50);
        mana = Random.Range(5, 50);
        physicalAtkDmg = Random.Range(5, 50);
        specialAtkDmg = Random.Range(5, 50);
        physicalDef = Random.Range(5, 50);
        specialDef = Random.Range(5, 50);
        speed = Random.Range(5, 50);
        critChance = Random.Range(5, 50);
        critDmg = Random.Range(5, 50);
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

public class ElementChart //Type effectiveness
{
    static float[][] chart =    //Column = attacker; Row = defender
    {
            //                          Hea   Ele  Rad  Ice  Lgt   Dar     
            /*Heat*/        new float[]{1f,   1f,  1f,  2f,  0.5f, 1f},
            /*Electric*/    new float[]{0.5f, 1f,  2f,  1f,  1f,   1f},
            /*Radiation*/   new float[]{1f,   2f,  1f,  1f,  1f,   1f},
            /*Ice*/         new float[]{0.5f, 1f,  2f,  0f,  1f,   0.5f},
            /*Light*/       new float[]{1f,   1f,  1f,  1f,  0f,   2f},
            /*Dark*/        new float[]{1f,   1f,  1f,  1f,  2f,   0f},
        };

    public static float ElementalModifier(elements attacker, elements defender)
    {
        int row = (int)attacker - 1;
        int col = (int)defender - 1;

        return chart[row][col];
    }
}

//Stats enum for other script ref
public enum Stat
{
    health,
    mana,
    physicalAtkDmg,
    specialAtkDmg,
    physicalDef,
    specialDef,
    speed,
    critChance,
    critDmg
}