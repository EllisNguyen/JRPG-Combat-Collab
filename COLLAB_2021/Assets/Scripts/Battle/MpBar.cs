using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MpBar : MonoBehaviour
{
    [SerializeField] Image mana;
    [SerializeField] TextMeshProUGUI manaTxt;

    public bool IsUpdating { get; private set; }

    public void SetMP(float hpNormalized, Character character)
    {
        mana.fillAmount = hpNormalized;
        manaTxt.text = $"{character.MP} / {character.MaxMP}";
    }

    public IEnumerator SetMPSmooth(float newMp, Character character)
    {
        IsUpdating = true;

        float curMp = mana.fillAmount;
        float changeAmt = curMp - newMp;

        manaTxt.text = $"{character.MP} / {character.MaxMP}";

        while (curMp - newMp > Mathf.Epsilon)
        {
            curMp -= changeAmt * Time.deltaTime;
            mana.fillAmount = curMp;
            yield return null;
        }
        mana.fillAmount = newMp;

        IsUpdating = false;
    }
}
