using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create new recovery item")]
public class Item_Heal : ItemBase
{
    [Header("HP restore")]
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHp;

    [Header("PP restore")]
    [SerializeField] int ppAmount;
    [SerializeField] bool restoreMaxPp;

    [Header("Status restore")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;

    [Header("Status restore")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevive;

    public override bool Use(Character character)
    {

        return false;

    }
}
