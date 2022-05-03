///Author: Phap Nguyen.
///Description: Pawn that hold information of the battle character.
///Day created: 27/11/2021
///Last edited: 03/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePawn : MonoBehaviour
{
    public SpriteRenderer graphic;

    [SerializeField] PlayerBase _base;
    [SerializeField] int level;

    public Character Character { get; set; }
    [SerializeField] bool isPlayerUnit;
    //Expose the isPlayerUnit property.
    public bool IsPlayerUnit
    {
        get
        {
            return isPlayerUnit;
        }
    }

    ConditionsDB condition;

    public void Setup(Character character)
    {
        Character = character;

        graphic.sprite = Character.Base.battleSprite;
    }
}
