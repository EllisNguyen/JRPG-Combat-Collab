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

    public void SetData(Move move)
    {
        skillName.text = move.Base.Name;
        manaRequirement.text = $"Mana req. {move.Base.Mana}";
        typeIcon.sprite = null;
    }
}
