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

    public override bool Use(Creature creature)
    {
        //USAGE OF REVIVE.
        if (revive || maxRevive)
        {
            if (creature.HP > 0)
                return false;

            //Revive and restore half HP.
            if (revive)
                creature.IncreaseHP(creature.MaxHP / 2);
            //Revive and restore full HP.
            else if (maxRevive)
                creature.IncreaseHP(creature.MaxHP);

            //Cure all status.
            creature.CureStatus();

            return true;
        }

        //Deny usage of other items when creature is fainted.
        if (creature.HP == 0)
            return false;

        //USAGE OF POTION.
        if (restoreMaxHp || hpAmount > 0)
        {
            //Do nothing if creature's health is already full.
            if (creature.HP == creature.MaxHP)
                return false;

            //Full potion.
            if (restoreMaxHp)
                creature.IncreaseHP(creature.MaxHP);
            else
                //else -> increase creature's HP.
                creature.IncreaseHP(hpAmount);
        }

        //USAGE OF STATUS REMOVER.
        if (recoverAllStatus || status != ConditionID.none)
        {
            //Check for status. Don't recover if no status.
            if (creature.Status == null && creature.VolatileStatus == null) return false;

            if (recoverAllStatus)
            {
                creature.CureStatus();
                creature.CureVolatileStatus();
            }
            else
            {
                //Check if the right Recovery medicine match the current status.
                if (creature.Status.Id == status) creature.CureStatus();
                else if (creature.VolatileStatus.Id == status) creature.CureVolatileStatus();
                else return false;
            }
        }

        //USAGE OF PP RESTORER.
        if (restoreMaxPp)
        {
            creature.Moves.ForEach(m => m.IncreasePP(m.Base.PP));
        }
        else if (ppAmount > 0)
        {
            creature.Moves.ForEach(m => m.IncreasePP(ppAmount));
        }

        return true;
    }
}
