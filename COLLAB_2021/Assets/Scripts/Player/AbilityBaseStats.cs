///Author: Quan.TM
///Description: Hold the stats for character abilities.
///Day created: 02/11/2021
///Last edited: DD/MM/YYYY - editor name.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AbilityBaseStats", menuName = "AbilityBaseStats")]

public class AbilityBaseStats : ScriptableObject
{
    [Header("Ability Stats")]
    public string abilityName;
    public string description;
    public float effectChance; //Volatile effect chance
    public float abilityAccuracy;
    public int baseDmg;
    public int manaUsage;
    public elements eles; //Reference to the elements enum

    public enum elements
    {
        normalAtk,
        heat,
        electric,
        ice,
        radiation,
        light,
        dark
    };

}

