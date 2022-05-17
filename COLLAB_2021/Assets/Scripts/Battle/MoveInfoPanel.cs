///Author: Phap Nguyen.
///Description: Show info of skill when hover on the skill button in battle.
///Day created: 16/05/2022
///Last edited: 17/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class MoveInfoPanel : MonoBehaviour
{
    Move _move;
    [SerializeField] Image typeIcon;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillDescription;
    [SerializeField] TextMeshProUGUI skillMana;
    [SerializeField] TextMeshProUGUI skillPower;
    [SerializeField] TextMeshProUGUI skillAccuracy;

    [FoldoutGroup("Attack type")][SerializeField] GameObject physicalAttack;
    [FoldoutGroup("Attack type")][SerializeField] GameObject specialAttack;
    [FoldoutGroup("Attack type")][SerializeField] GameObject statusEffect;

    public void SetData(Move move)
    {
        skillName.text = move.Base.Name;
        skillPower.text = $"Power: { move.Base.Power}";
        skillAccuracy.text = $"Accuracy: { move.Base.Accuracy}";
        skillDescription.text = move.Base.Description;
        skillMana.text = $"MP requires: { move.Base.Mana}";

        //Set element icon.
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

        //Set move category.
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
}
