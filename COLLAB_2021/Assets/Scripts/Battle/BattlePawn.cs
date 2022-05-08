///Author: Phap Nguyen.
///Description: Pawn that hold information of the battle character.
///Day created: 27/11/2021
///Last edited: 03/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;

public class BattlePawn : MonoBehaviour
{
    public bool isActive = false;

    public SpriteRenderer graphic;

    [SerializeField] CharacterBaseStats _base;
    [SerializeField] int level;
    [SerializeField] BattleHud hud;

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

    //Expose the hud property.
    public BattleHud Hud
    {
        get { return hud; }
        set { hud = value; }
    }

    ConditionsDB condition;
    Vector3 originalPos;

    [FoldoutGroup("Combat Boost Particle")]public ParticleSystem levelUp;
    [FoldoutGroup("Combat Boost Particle")]public ParticleSystem atkUp;
    [FoldoutGroup("Combat Boost Particle")]public ParticleSystem spatkUp;
    [FoldoutGroup("Combat Boost Particle")]public ParticleSystem defUp;
    [FoldoutGroup("Combat Boost Particle")]public ParticleSystem sfpdefUp;

    public void Setup(Character character)
    {
        Character = character;

        graphic.sprite = Character.Base.battleSprite;

        if (isPlayerUnit)
            gameObject.transform.localPosition = new Vector3(-10f, originalPos.y);
        else
            gameObject.transform.localPosition = new Vector3(10f, originalPos.y);

        PlayEnterAnimation();
    }

    //Animation when Character enter the battle.
    public void PlayEnterAnimation()
    {
        //Set position when battle start.
        gameObject.transform.DOLocalMoveX(originalPos.x, Random.Range(0.5f,1.25f));
    }

    //Animation when Character enter the battle.
    public void PlayFleeAnimation()
    {
        ////Set position when battle start.
        //if (isPlayerUnit)
        //    gameObject.transform.localPosition = new Vector3(originalPos.y, -10f);

        //if (isPlayerUnit)
            gameObject.transform.DOLocalMoveX(-10f, Random.Range(0.5f, 1.25f));
    }

    public void PlayAttackAnimation()
    {
        //Declare a dotween sequence func.
        var sequence = DOTween.Sequence();

        //Do a slight movement.
        if (isPlayerUnit)
            sequence.Append(gameObject.transform.DOLocalMoveX(originalPos.x + 3f, 0.15f));
        else
            sequence.Append(gameObject.transform.DOLocalMoveX(originalPos.x - 3f, 0.15f));

        //Return to original position.
        sequence.Append(gameObject.transform.DOLocalMoveX(originalPos.x, 0.15f));
    }

    public void PlayFaintAnimation()
    {
        //Declare a dotween sequence func.
        var sequence = DOTween.Sequence();

        //Play sequence of move and fade altogether.
        sequence.Append(gameObject.transform.DOLocalMoveY(originalPos.y - 10f, 0.15f));
        sequence.Join(graphic.DOFade(0, 0.1f));
    }

    public void PlayHitAnimation()
    {
        //Declare a dotween sequence func.
        var sequence = DOTween.Sequence();

        //Flick color when hit.
        sequence.Append(graphic.DOColor(Color.red, 0.1f));

        //Return to original color.
        sequence.Append(graphic.DOColor(Color.white, 0.1f));
    }
}
