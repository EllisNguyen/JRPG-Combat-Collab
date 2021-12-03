///Author: Phap Nguyen.
///Description: Scriptable object that hold the object data for the item.
///Day created: 27/11/2021
///Last edited: 28/11/2021 - Phap Nguyen.

using UnityEngine;
using Sirenix.OdinInspector;

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
}
