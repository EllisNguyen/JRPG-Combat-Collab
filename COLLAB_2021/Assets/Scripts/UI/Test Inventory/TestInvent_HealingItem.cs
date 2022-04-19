using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("This is the scriptable object that held the base info for healing item. This item only have 2 attributes.")]
public class TestInvent_HealingItem : TestInvent_Item
{

    [SerializeField] int healAmount;
    [SerializeField] int healOverNumberOfTurn;

    public override bool Use(Character character)
    {
        return false;
    }
}
