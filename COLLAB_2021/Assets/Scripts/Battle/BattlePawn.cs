using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePawn : MonoBehaviour
{
    public SpriteRenderer graphic;

    [SerializeField] CharacterBaseStats _base;
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

    public void Setup()
    {
        Character = new Character(_base, level);

        graphic.sprite = Character.Base.battleSprite;
    }
}
