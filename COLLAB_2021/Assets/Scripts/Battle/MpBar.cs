using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour
{
    [SerializeField] Image mana;

    public bool IsUpdating { get; private set; }

    public void SetMP(float hpNormalized)
    {
        mana.fillAmount = hpNormalized;
    }

    public IEnumerator SetMPSmooth(float newMp)
    {
        IsUpdating = true;

        float curMp = mana.fillAmount;
        float changeAmt = curMp - newMp;

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
