///Author: Phap Nguyen.
///Description: Scriptable object that hold the object data for the item.
///Day created: 27/11/2021
///Last edited: 28/11/2021 - Phap Nguyen.

using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TEST", menuName = "TEST/Create TEST ITEM")]
[TypeInfoBox("This is the scriptable object and also a parent class for all  type of items.")]
public class TestInvent_Item : ScriptableObject
{
    [ShowInInspector, PropertySpace(16)]

    //ABOUT
    [Header("VISUAL STUFF")]
    [InfoBox("Sum visual stuff, keep cool :)")]
    [SerializeField] string itemName;
    [TextArea][SerializeField] string itemDescription;
    [SerializeField] Sprite itemSprite;

    //Expose ABOUT var.
    public string NAME => itemName;
    public string DESCRIPTION => itemDescription;
    public Sprite SPRITE => itemSprite;

    [ShowInInspector, PropertySpace(16)]

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


    /// <summary>
    /// Check if item used on any character.
    /// Can be override by other child class(es).
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public virtual bool Use(Character character)
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
