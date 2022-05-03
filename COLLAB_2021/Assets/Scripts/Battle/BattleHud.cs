///Author: Phap Nguyen.
///Description: The panel that hold the information of battle pawns, like: name, level, hp, mp and exp if it the player.
///Day created: 25/04/2022
///Last edited: 03/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HpBar hpBar;
    [SerializeField] MpBar mpBar;
    [SerializeField] GameObject expBar;

    /// <summary>
    /// Set correct data of the BattleHud.
    /// Pull information from the party on of the battle units.
    /// </summary>
    /// <param name="character"></param>
    public void SetData(Character character)
    {
        nameText.text = character.Base.charName;
        levelText.text = $"<size=9>Level</size>\n{ character.Level}";
        hpBar.SetHP((float)character.HP / character.MaxHP);
        mpBar.SetMP((float)character.MP / character.MaxMP);
    }

    /// <summary>
    /// Disable the expbar if the hud owns by a non-player unit.
    /// </summary>
    public void DisableNonPlayerElement()
    {
        expBar.SetActive(false);
    }
}
