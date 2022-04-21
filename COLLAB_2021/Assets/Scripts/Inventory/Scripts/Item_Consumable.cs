using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create consumable item")]
public class Item_Consumable : ItemBase
{
    [Header("HP restore")]
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHp;
    
    [Header("MP restore")]
    [SerializeField] int mpAmount;
    [SerializeField] bool restoreMaxMp;

    [Header("PP restore")]
    [SerializeField] int ppAmount;
    [SerializeField] bool restoreMaxPp;

    [Header("Status restore")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;

    [Header("Status restore")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevive;

    [Header("Stat boost")]
    [SerializeField] int atkBoostAmount;
    [SerializeField] int defBoostAmount;
    [SerializeField] int spatkBoostAmount;
    [SerializeField] int spdefBoostAmount;
    [SerializeField] int spdBoostAmount;
    [SerializeField] int critRateBoostAmount;
    [SerializeField] bool isPermanentBoost = false;

    public override bool Use(Character character)
    {
        return false;
    }

    //TODO: add more custom behaviours for the consumable here.

    /*
    Custom behaviours like: (example)
    -What happen when use the item: maybe display a dialogue to indicate that the item has been used.
    -
    */
}
