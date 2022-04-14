///Author: Phap Nguyen.
///Description: Scriptable object that hold the object data for the item.
///Day created: 27/11/2021
///Last edited: 28/11/2021 - Phap Nguyen.

using System.Collections.Generic;
using UnityEngine;

//List of enum for rarity of the item.
public enum ItemRarity { Common, Uncommon, Rare, Legendary, Exotic }

public enum ItemInteraction { None , Use, Consume, Equip}

public class ItemBase : ScriptableObject
{
    //ABOUT
    [Header("VISUAL STUFF")]
    public ItemRarity rarity;
    [SerializeField] string itemName;//Item name.
    [TextArea] [SerializeField] string itemDescription;//Item description.
    [SerializeField] Sprite itemSprite;//Item sprite.
    [SerializeField] int sellPrice;//Sell price.
    [SerializeField] int buyPrice;//Buy price.
    [SerializeField] bool stackable;//Stackable?
    Color rarityColor;

    public ItemInteraction interactionKey;

    [SerializeField] List<CoreAttribute> coreAttributes;

    //Expose the private variables.
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public int SellPrice => sellPrice;
    public int BuyPrice => buyPrice;
    public bool Stackable => stackable;
    public Sprite ItemSprite => itemSprite;
    public List<CoreAttribute> CoreAttributes => coreAttributes;

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

    /// <summary>
    /// Update the attribute name on changing the attribute type enum.
    /// </summary>
    void OnValidate()
    {
        for (int i = 0; i < CoreAttributes.Count; i++)
        {
            CoreAttributes[i].AttributeName = CoreAttributes[i].SetAttribute(CoreAttributes[i].AttributeType);
        }
    }

    public Color GetRarityColor(ItemBase item)
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                rarityColor = GameManager.Instance.common;
                break;
            case ItemRarity.Uncommon:
                rarityColor = GameManager.Instance.uncommon;
                break;
            case ItemRarity.Rare:
                rarityColor = GameManager.Instance.rare;
                break;
            case ItemRarity.Legendary:
                rarityColor = GameManager.Instance.legendary;
                break;
            case ItemRarity.Exotic:
                rarityColor = GameManager.Instance.exotic;
                break;
            default:
                break;
        }

        return rarityColor;
    }
}

/// <summary>
/// Public class :)
/// </summary>
[System.Serializable]
public class CoreAttribute
{
    [SerializeField] AttributeType attributeType;
    [SerializeField] string attributeName;
    [Range(0, 100)] [SerializeField] int attributeGain;

    public string SetAttribute(AttributeType type)
    {
        switch (attributeType)
        {
            //REGEN ATTRIBUTE.
            case AttributeType.HealthRegen:
                attributeName = "Health Regen";
                break;
            case AttributeType.ManaRegen:
                attributeName = "Mana Regen";
                break;

            //ATTACK ATTRIBUTE.
            case AttributeType.Atk1:
                attributeName = "ATK+";
                break;            
            case AttributeType.Atk2:
                attributeName = "ATK++";
                break;            
            case AttributeType.Atk3:
                attributeName = "ATK+++";
                break;

            //DEFENSE ATTRIBUTE.
            case AttributeType.Def1:
                attributeName = "DEF+";
                break;
            case AttributeType.Def2:
                attributeName = "DEF++";
                break;
            case AttributeType.Def3:
                attributeName = "DEF+++";
                break;

            //SPECIAL ATTACK ATTRIBUTE.
            case AttributeType.SpAtk1:
                attributeName = "Sp.ATK+";
                break;
            case AttributeType.SpAtk2:
                attributeName = "Sp.ATK++";
                break;
            case AttributeType.SpAtk3:
                attributeName = "Sp.ATK+++";
                break;

            //SPECIAL DEFENSE ATTRIBUTE.
            case AttributeType.SpDef1:
                attributeName = "Sp.DEF+";
                break;
            case AttributeType.SpDef2:
                attributeName = "Sp.DEF++";
                break;
            case AttributeType.SpDef3:
                attributeName = "Sp.DEF+++";
                break;

            //SPEED ATTRIBUTE.
            case AttributeType.Spd1:
                attributeName = "SPD+";
                break;
            case AttributeType.Spd2:
                attributeName = "SPD++";
                break;
            case AttributeType.Spd3:
                attributeName = "SPD+++";
                break;

            //CRITICAL ATTRIBUTE.
            case AttributeType.CritRateUp:
                attributeName = "Crit. Rate";
                break;
            case AttributeType.CritDmgUp:
                attributeName = "Crit. Damage";
                break;
            default:
                break;
        }

        return attributeName;
    }

    //Publicly expose attributeName string, free to ref and set.
    public string AttributeName
    {
        get { return attributeName; }
        set => attributeName = value;
    }

    //Publicly expose attributeGain int, free to ref and set.
    public int AttributeGain
    {
        get { return attributeGain; }
        set => attributeGain = value;
    }

    public AttributeType AttributeType => attributeType;
}

//List of attribute type for the item.
public enum AttributeType
{
    //Regen Item
    HealthRegen, ManaRegen,

    //ATK Boost
    Atk1, Atk2, Atk3,

    //DEF Boost
    Def1, Def2, Def3,

    //SP.ATK Boost
    SpAtk1, SpAtk2, SpAtk3,

    //SP.DEF Boost
    SpDef1, SpDef2, SpDef3,

    //SPD Boost
    Spd1, Spd2, Spd3,

    //CRIT Boost
    CritRateUp,
    CritDmgUp,
}