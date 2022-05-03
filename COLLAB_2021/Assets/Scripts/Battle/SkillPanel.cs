///Author: Phap Nguyen.
///Description: Button panel for the skill.
///Day created: 20/01/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPanel : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI manaRequirement;
    [SerializeField] Image typeIcon;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="move"></param>
    public void SetData(Move move)
    {
        skillName.text = move.Base.Name;
        manaRequirement.text = $"Mana req. {move.Base.Mana}";
        typeIcon.sprite = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="move"></param>
    /// <param name="receiver"></param>
    /// <param name="target"></param>
    public void DoSkill(Move move, BattlePawn receiver, bool target)
    {

    }
}
