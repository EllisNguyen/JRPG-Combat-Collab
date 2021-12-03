///Author: Quan.TM
///Description: Hold the stats for characters and attributes.
///Day created: 02/11/2021
///Last edited: 14/11/2021 - Phab Nguyen.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterBaseStats", menuName = "CharacterBaseStats")]

public class CharacterBaseStats : ScriptableObject
{
    #region PlayerStats
    [Header("Player Stats")]
    public string charName;
    public int health;
    public int mana;
    public int physicalAtkDmg;
    public int specialAtkDmg;
    public int physicalDef;
    public int specialDef;
    public int speed;
    public int critChance;
    public int critDmg;
    public int elementalRes;
    public int exp;
    public elements eles; //Reference to the elements enum
    #endregion

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
        static float[][] chart =                                        //Column = attacker; Row = defender
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