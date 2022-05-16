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
using Sirenix.OdinInspector;

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
    [SerializeField] Image baseImage;
    Button button;

    [FoldoutGroup("Attack type")][SerializeField] GameObject physicalAttack;
    [FoldoutGroup("Attack type")][SerializeField] GameObject specialAttack;
    [FoldoutGroup("Attack type")][SerializeField] GameObject statusEffect;

    public Button Button => button;

    BattleSystem battleSystem;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="move"></param>
    public void SetData(Move move)
    {
        _move = move;
        skillName.text = move.Base.Name;

        gameObject.name = move.Base.Name;

        manaRequirement.text = $"{move.Base.Mana} MP";
        typeIcon.sprite = null;
        battleSystem = FindObjectOfType<BattleSystem>();

        switch (move.Base.Element)
        {
            case elements.normal:
                typeIcon.sprite = GameController.Instance.normalIcon;
                typeIcon.color = GameController.Instance.normal;
                break;
            case elements.heat:
                typeIcon.sprite = GameController.Instance.heatIcon;
                typeIcon.color = GameController.Instance.heat;
                break;
            case elements.electric:
                typeIcon.sprite = GameController.Instance.electricIcon;
                typeIcon.color = GameController.Instance.electric;
                break;
            case elements.radiation:
                typeIcon.sprite = GameController.Instance.radiationIcon;
                typeIcon.color = GameController.Instance.radiation;
                break;
            case elements.ice:
                typeIcon.sprite = GameController.Instance.iceIcon;
                typeIcon.color = GameController.Instance.ice;
                break;
            case elements.light:
                typeIcon.sprite = GameController.Instance.lightIcon;
                typeIcon.color = GameController.Instance.light;
                break;
            case elements.dark:
                typeIcon.sprite = GameController.Instance.darkIcon;
                typeIcon.color = GameController.Instance.dark;
                break;
            default:
                break;
        }

        switch (move.Base.Category)
        {
            case MoveCategory.Physical:
                physicalAttack.SetActive(true);
                specialAttack.SetActive(false);
                statusEffect.SetActive(false);
                break;
            case MoveCategory.Range:
                physicalAttack.SetActive(false);
                specialAttack.SetActive(true);
                statusEffect.SetActive(false);
                break;
            case MoveCategory.Status:
                physicalAttack.SetActive(false);
                specialAttack.SetActive(false);
                statusEffect.SetActive(true);
                break;
            default:
                break;
        }
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
        //StartCoroutine(battleSystem.RunTurnsPlayer(BattleAction.Move));
        //battleSystem.DialogueBox.EnableActionSelector(false);
        battleSystem.PlayerPerform = true;
        battleSystem.State = BattleState.RunningTurn;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        battleSystem.currentMovePanel = this;
        battleSystem.currentMove = Move;
        battleSystem.MoveInfoPanel.gameObject.SetActive(true);
        battleSystem.MoveInfoPanel.SetData(_move);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //battleSystem.currentMovePanel = null;
        battleSystem.MoveInfoPanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        battleSystem.MoveInfoPanel.gameObject.SetActive(false);
        //TODO: Fight.
        //battleSystem.UseSKill()
        //StartCoroutine(battleSystem.RunTurnsPlayer(BattleAction.Move));

        //if (battleSystem.ActiveUnit.IsPlayerUnit) StartCoroutine(battleSystem.RunTurnsPlayer(BattleAction.Move));
        //else StartCoroutine(battleSystem.RunTurnsEnemy(BattleAction.Wait));
    }
}
