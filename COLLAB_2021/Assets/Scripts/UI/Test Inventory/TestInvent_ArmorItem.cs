using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[TypeInfoBox("This is the scriptable object that held the base info for armor item. This item have 6 attributes.")]
public class TestInvent_ArmorItem : TestInvent_Item
{
    //STATS
    [Header("STAT STUFF")]
    [InfoBox("Sum stat stuff, number and shit :)")]
    [Range(0, 100)] [SerializeField] int physAtk;
    [Range(0, 100)] [SerializeField] int physDef;
    [Range(0, 100)] [SerializeField] int specAtk;
    [Range(0, 100)] [SerializeField] int specDef;
    [Range(0, 100)] [SerializeField] int speed;
    [Range(0, 100)] [SerializeField] int critChance;

    //Expose STATS var.
    public int PHYS_ATK => physAtk;
    public int PHYS_DEF => physDef;
    public int SPEC_ATK => specAtk;
    public int SPEC_DEF => specDef;
    public int SPEED => speed;
    public int CRIT_CHANCE => critChance;

    public override bool Use(Character character)
    {
        return false;
    }

    //Randomizer button in the inspector for the stats.
    [Button]
    private void RandomAttributes()
    {
        physAtk = Random.Range(0, 100);
        physDef = Random.Range(0, 100);

        specAtk = Random.Range(0, 100);
        specDef = Random.Range(0, 100);

        speed = Random.Range(0, 100);
        critChance = Random.Range(0, 100);
    }
}
