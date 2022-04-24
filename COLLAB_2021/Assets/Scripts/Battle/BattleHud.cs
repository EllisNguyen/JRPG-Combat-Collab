using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] HpBar hpBar;
    [SerializeField] MpBar mpBar;
    [SerializeField] GameObject expBar;

    public void SetData(Character character)
    {
        nameText.text = character.Base.charName + " - Lv." + character.Level;
        hpBar.SetHP((float)character.HP / character.MaxHP);
        mpBar.SetMP((float)character.MP / character.MaxMP);
    }

    public void DisableNonPlayerElement()
    {
        expBar.SetActive(false);
    }
}
