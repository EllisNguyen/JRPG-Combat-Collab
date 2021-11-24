using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour
{

    Image faderImage;

    private void Awake()
    {
        faderImage = GetComponent<Image>();
    }

    public IEnumerator FadeIn(float time)
    {
        yield return faderImage.DOFade(1f, time).WaitForCompletion();
    }

    public IEnumerator FadeOut(float time)
    {
        yield return faderImage.DOFade(0f, time).WaitForCompletion();
    }
}
