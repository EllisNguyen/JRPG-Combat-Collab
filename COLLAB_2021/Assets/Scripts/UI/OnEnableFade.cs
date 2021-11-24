//Author: Phap Nguyen
//Description: Do the fade in effect when UI panel is enabled

using UnityEngine;
using DG.Tweening;

public class OnEnableFade : MonoBehaviour
{
    public enum Component
    {
        CanvasGroup,
        Music
    }
    public Component component = Component.CanvasGroup;

    public float fadeTime = 0.2f;

    void OnEnable()
    {
        switch (component)
        {
            case Component.CanvasGroup:
                var panel = gameObject.GetComponent<CanvasGroup>();
                panel.DOFade(1, fadeTime);
                break;
            case Component.Music:
                break;
            default:
                component = Component.CanvasGroup;
                break;
        }
    }

    void OnDisable()
    {
        var panel = gameObject.GetComponent<CanvasGroup>();
        panel.alpha = 0;
        //panel.DOFade(0, fadeTime);
    }
}
