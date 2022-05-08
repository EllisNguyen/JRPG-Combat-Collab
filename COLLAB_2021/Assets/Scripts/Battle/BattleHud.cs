///Author: Phap Nguyen.
///Description: The panel that hold the information of battle pawns, like: name, level, hp, mp and exp if it the player.
///Day created: 25/04/2022
///Last edited: 03/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class BattleHud : MonoBehaviour
{
    Character _character;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HpBar hpBar;
    [SerializeField] MpBar mpBar;
    [SerializeField] TextMeshProUGUI statusText;

    [BoxGroup("EXP BAR")] [SerializeField] GameObject expContainer;
    [BoxGroup("EXP BAR")] [SerializeField] Image expBar;

    [Header("Status Colors")]
    [SerializeField] Color radColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color bldColor;
    [SerializeField] Color parColor;
    [SerializeField] Color decColor;

    //Store all color in Dictionary.
    Dictionary<ConditionID, Color> statusColor;

    /// <summary>
    /// Set correct data of the BattleHud.
    /// Pull information from the party on of the battle units.
    /// </summary>
    /// <param name="character"></param>
    public void SetData(Character character)
    {
        if (_character != null)
        {
            //Unsub the OnHPChanged and OnStatusChanged Action.
            _character.OnHPChanged -= UpdateHP;
            _character.OnHPChanged -= UpdateMP;
            _character.OnStatusChanged -= SetStatusText;
        }

        _character = character;

        nameText.text = character.Base.charName;
        SetLevel();
        hpBar.SetHP((float)character.HP / character.MaxHP, character);
        mpBar.SetMP((float)character.MP / character.MaxMP, character);

        SetExp();

        statusColor = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.rad, radColor },
            {ConditionID.brn, brnColor },
            {ConditionID.par, parColor },
            {ConditionID.bln, bldColor },
            {ConditionID.dec, decColor },
        };

        //Change status image base on the status condition.
        SetStatusText();

        //Maybe reset stats here.

        //Let the OnStatusChanged Action trigger the SetStatusImage().
        _character.OnStatusChanged += SetStatusText;

        //Let the OnHPChanged Action trigger the UpdateHP().
        _character.OnHPChanged += UpdateHP;
        _character.OnHPChanged += UpdateMP;
    }

    /// <summary>
    /// Disable the expbar if the hud owns by a non-player unit.
    /// </summary>
    public void DisableNonPlayerElement()
    {
        expContainer.SetActive(false);
    }

    //Set new level for the creature
    public void SetLevel()
    {
        //Set text level
        levelText.text = $"<size=9>Level</size>\n{_character.Level}";
    }

    //Increase EXP instantly.
    public void SetExp()
    {
        //Skip setting exp for opponent.
        if (expBar == null) return;

        float normalizedExp = GetNormalizedExp();
        expBar.fillAmount = normalizedExp;
    }

    //Increase EXP smoothly.
    public IEnumerator SetExpSmooth(bool reset = false)
    {
        //Skip setting exp for opponent.
        if (expBar == null) yield break;

        //Reset progress bar.
        if (reset)
        {
            expBar.fillAmount = 0;
        }

        float normalizedExp = GetNormalizedExp();
        //yield return expBar.gameObject.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
        while (expBar.fillAmount < normalizedExp)
        {
            expBar.fillAmount += 0.25f * Time.deltaTime;
            yield return null;
        }
        expBar.fillAmount = normalizedExp;
        yield return new WaitUntil(() => expBar.fillAmount == normalizedExp);
    }

    float GetNormalizedExp()
    {
        //Declare the current exp.
        int currentLevelExp = _character.Base.GetExpForLevel(_character.Level);

        //Exp needed to level up.
        int nextLevelExp = _character.Base.GetExpForLevel(_character.Level + 1);

        //Normalized current exp.
        float normalizedExp = (float)(_character.Exp - currentLevelExp) / (nextLevelExp - currentLevelExp);

        return Mathf.Clamp01(normalizedExp);
    }

    //Set the status text on the HUD.
    void SetStatusText()
    {
        if (_character.Status == null)
        {
            //Remove status text.
            statusText.text = "";
        }
        else
        {
            //Set status text and color.
            statusText.text = _character.Status.Name.ToUpper();
            statusText.color = statusColor[_character.Status.Id];

            //statusSprite.sprite = statusImages[_creature.Status.Id];
        }
    }

    public void UpdateHP()
    {
        StartCoroutine(UpdateHPAsync());
    }

    public IEnumerator UpdateHPAsync()
    {
        //Set current health on health bar
        yield return hpBar.SetHPSmooth((float)_character.HP / _character.MaxHP, _character);
    }

    public void UpdateMP()
    {
        StartCoroutine(UpdateMPAsync());
    }

    public IEnumerator UpdateMPAsync()
    {
        //Set current health on health bar
        yield return mpBar.SetMPSmooth((float)_character.MP / _character.MaxMP, _character);
    }

    public IEnumerator WaitForHpUpdate()
    {
        yield return new WaitUntil(() => hpBar.IsUpdating == false);
    }

    public void ClearData()
    {
        if (_character != null)
        {
            //Unsub the OnHPChanged and OnStatusChanged Action.
            _character.OnHPChanged -= UpdateHP;
            _character.OnHPChanged -= UpdateMP;
            _character.OnStatusChanged -= SetStatusText;
        }
    }
}
