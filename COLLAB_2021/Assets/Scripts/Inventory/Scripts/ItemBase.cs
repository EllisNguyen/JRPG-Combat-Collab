///Author: Phap Nguyen.
///Description: Scriptable object that hold the object data for the item.
///Day created: 27/11/2021
///Last edited: 28/11/2021 - Phap Nguyen.

using System.Collections.Generic;
using UnityEngine;

//List of enum for rarity of the item.
public enum ItemRarity { Common, Uncommon, Rare, Legendary, Exotic }

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
            case AttributeType.HealthRegen:
                attributeName = "Health Regen";
                break;
            case AttributeType.ManaRegen:
                attributeName = "Mana Regen";
                break;
            case AttributeType.AtkUp:
                attributeName = "Attack";
                break;
            case AttributeType.DefUp:
                attributeName = "Defense";
                break;
            case AttributeType.SpAtkUp:
                attributeName = "Sp. Attack";
                break;
            case AttributeType.SpDefUp:
                attributeName = "Sp. Defense";
                break;
            case AttributeType.SpdUp:
                attributeName = "Speed";
                break;
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

    public string AttributeName
    {
        get { return attributeName; }
        set => attributeName = value;
    }

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
    HealthRegen,
    ManaRegen,

    //Armor Boost
    AtkUp,
    DefUp,
    SpAtkUp,
    SpDefUp,
    SpdUp,
    CritRateUp,
    CritDmgUp,
}