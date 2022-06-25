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
    [SerializeField] SpeedProgressor progressor;
    [SerializeField] Transform cqcPos;
    [SerializeField] Transform lookatPos;
    [SerializeField] List<Move> skills;
    [SerializeField] Color hitColor;
    [SerializeField] public AudioSource levelUpAudio;
    public Transform CloseCombatPos => cqcPos;
    public Transform LookAtPos => lookatPos;

    public SpeedProgressor Progressor
    {
        get { return progressor; }
        set { progressor = value; }
    }

    public List<Move> Skills
    {
        get { return skills; }
        set { skills = value; }
    }

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
    [FoldoutGroup("Combat Boost Particle")]public ParticleSystem statUp;
    [FoldoutGroup("Combat Boost Particle")]public ParticleSystem statDown;

    public void Setup(Character character)
    {
        Character = character;
        _base = character.Base;
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

    public void PlayGuardAnimation(Move move, BattlePawn target)
    {
        if (_base.guardSprite != null)
        {
            StartCoroutine(GuardSequence(move, target));
        }
        else
        {
            gameObject.transform.DOLocalJump(gameObject.transform.position, 0.4f, 1, 0.15f, true);
        }
    }

    IEnumerator GuardSequence(Move move, BattlePawn target)
    {
        graphic.sprite = _base.guardSprite;

        yield return new WaitForSeconds(0.85f);

        graphic.sprite = _base.battleSprite;
    }

    public void PlayAttackAnimation(Move move, BattlePawn target)
    {
        if(_base.attackSprite != null)
        {
            if (move.Base.Category == MoveCategory.Physical)
                StartCoroutine(CloseCombatSequence(move, target));
            else
                StartCoroutine(RangeCombatSequence(move, target));
        }
        else
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
    }

    IEnumerator CloseCombatSequence(Move move, BattlePawn target)
    {
        Vector3 curPos = this.transform.position;
        Transform targetPos = target.CloseCombatPos;
        var sequence = DOTween.Sequence();

        sequence.Append(gameObject.transform.DOMove(targetPos.position, 0.035f));
        graphic.sprite = _base.attackSprite;

        yield return new WaitForSeconds(0.55f);

        sequence.Append(gameObject.transform.DOMove(curPos, 0.15f));
        graphic.sprite = _base.battleSprite;
    }

    IEnumerator RangeCombatSequence(Move move, BattlePawn target)
    {
        graphic.sprite = _base.attackSprite;

        yield return new WaitForSeconds(0.55f);

        graphic.sprite = _base.battleSprite;
    }

    public void PlayFaintAnimation()
    {
        //Declare a dotween sequence func.
        var sequence = DOTween.Sequence();
        graphic.color = Color.red;

        //Do a slight movement.
        if (isPlayerUnit)
            sequence.Append(gameObject.transform.DOLocalMoveX(originalPos.x - 1.5f, 1f));
        else
            sequence.Append(gameObject.transform.DOLocalMoveX(originalPos.x + 1.5f, 1f));

        sequence.Join(graphic.DOFade(0, 1f));
    }

    public void PlayHitAnimation()
    {
        //Declare a dotween sequence func.
        var sequence = DOTween.Sequence();

        //Flick color when hit.
        sequence.Append(graphic.DOColor(hitColor, 0.1f));

        //Return to original color.
        sequence.Append(graphic.DOColor(Color.white, 0.1f));

        //Flick color when hit.
        sequence.Append(graphic.DOColor(hitColor, 0.1f));

        //Return to original color.
        sequence.Append(graphic.DOColor(Color.white, 0.1f));
    }

    public void PlayLevelUpAudio()
    {
        levelUpAudio.Play();
    }
}
