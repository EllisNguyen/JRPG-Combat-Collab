using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePawn : MonoBehaviour
{
    public SpriteRenderer graphic;

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

    public TextMeshProUGUI charName;
    public TextMeshProUGUI charLevel;
    public Image healthBar;
    ConditionsDB condition;

    public void Setup(Character character)
    {
        //Apply sprite correctly for player's and opponent's.
        Character = character;

        graphic.sprite = Character.Base.battleSprite;
    }
}
