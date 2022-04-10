using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Image health;

    public bool IsUpdating { get; private set; }

    public void SetHP(float hpNormalized)
    {
        health.fillAmount = hpNormalized;
    }

    public IEnumerator SetHPSmooth(float newHp)
    {
        IsUpdating = true;

        float curHp = health.fillAmount;
        float changeAmt = curHp - newHp;

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
