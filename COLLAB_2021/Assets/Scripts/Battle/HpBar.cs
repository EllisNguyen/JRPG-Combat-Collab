using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBar : MonoBehaviour
{
    [SerializeField] Image health;
    [SerializeField] TextMeshProUGUI healthTxt;

    public bool IsUpdating { get; private set; }

    public void SetHP(float hpNormalized, Character character)
    {
        health.fillAmount = hpNormalized;
        healthTxt.text = $"{character.HP} / {character.MaxHP}";
    }

    public IEnumerator SetHPSmooth(float newHp, Character character)
    {
        IsUpdating = true;

        float curHp = health.fillAmount;
        float changeAmt = curHp - newHp;

        healthTxt.text = $"{character.HP} / {character.MaxHP}";

        while (curHp - newHp > Mathf.Epsilon)
        {
            curHp -= changeAmt * Time.deltaTime;
            health.fillAmount = curHp;
            yield return null;
        }
        health.fillAmount = newHp;

        IsUpdating = false;
    }
}
