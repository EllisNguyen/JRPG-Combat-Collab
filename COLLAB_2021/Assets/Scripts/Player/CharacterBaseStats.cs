///Author: Quan.TM
///Description: Hold the stats for characters and attributes.
///Day created: 02/11/2021
///Last edited: 28/03/2021 - Phab Nguyen.

using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterBaseStats", menuName = "CharacterBaseStats")]

public class CharacterBaseStats : ScriptableObject
{
    #region PlayerStats
    [Header("Player Stats")]
    public string charName;
    public Sprite portraitSprite;
    public Sprite battleSprite;
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

    public void OnValidate()
    {
        charName = name;
    }
}

public enum elements
{
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
}