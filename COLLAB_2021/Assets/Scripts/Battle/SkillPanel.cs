///Author: Phap Nguyen.
///Description: Button panel for the skill.
///Day created: 20/01/2022
///Last edited: 05/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Move _move;

    public Move Move
    {
        get { return _move; }
    }

    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI manaRequirement;
    [SerializeField] Image typeIcon;

    BattleSystem battleSystem;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="move"></param>
    public void SetData(Move move)
    {
        _move = move;
        skillName.text = move.Base.Name;

        gameObject.name = move.Base.Name;

        manaRequirement.text = $"Mana req. {move.Base.Mana}";
        typeIcon.sprite = null;
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="move"></param>
    /// <param name="receiver"></param>
    public void DoSkill(Move move, BattlePawn receiver)
    {

    }

    public void DoSkill()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        battleSystem.currentMovePanel = this;
        battleSystem.currentMove = _move;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        battleSystem.currentMovePanel = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: Fight.
        //battleSystem.UseSKill()
        StartCoroutine(battleSystem.RunTurnsPlayer(BattleAction.Move));

        //if (battleSystem.ActiveUnit.IsPlayerUnit) StartCoroutine(battleSystem.RunTurnsPlayer(BattleAction.Move));
        //else StartCoroutine(battleSystem.RunTurnsEnemy(BattleAction.Wait));
    }
}
