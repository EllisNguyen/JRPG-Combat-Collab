using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Slider))]
public class TestProgressor : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TestParty party;
    public float value = 0;

    public void DoProgress(Character character)
    {
        if (party.activeUnit != null) return;

        //float value = 0;
        DOTween.To(() => value, x => value = x, slider.maxValue, (character.Speed * 0.5f))
            .OnUpdate(() =>
            {
                print($"{character.Base.charName}'s speed is {character.Speed}");
                slider.value = value;
                //yield return null;
            })
            .OnComplete(() =>
            {
                party.activeUnit = this;
                DOTween.PauseAll();
            });
    }

    public void ResetSlider()
    {
        value = 0;
        slider.value = 0;
        DOTween.PlayAll();
    }
}
