using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuickAnim : MonoBehaviour
{
    [SerializeField] Image animObject;
    [SerializeField] AnimationTypes animationTypes;
    [SerializeField] float timer;
    public Ease EaseType;

    // Start is called before the first frame update
    void Start()
    {
        switch (animationTypes)
        {
            case AnimationTypes.None:
                break;
            case AnimationTypes.Blinking:
                animObject.DOFade(0.0f, timer).SetEase(this.EaseType).SetLoops(-1, LoopType.Yoyo);
                break;
            default:
                break;
        }
    }
}

public enum AnimationTypes { None, Blinking}