///Author: Phab Nguyen.
///Description: this script act as a replace of the Unity button.
///Day created: 07/08/2021

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class P_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Get the rect transform of the button object.
    [SerializeField] RectTransform buttonRect;

    //BUTTON ANIMATION CONTROL
    public enum ButtonAnimation { Fade, Move, Scale, FadeAndMove, FadeAndScale, MoveAndScale }
    public ButtonAnimation buttonAnimation;
    [SerializeField] Vector2 buttonRectScale;
    [SerializeField] Image buttonGraphic;
    [SerializeField] Vector2 scaleIncrement = new Vector2(10, 10);
    [SerializeField] float buttonAnimationTime = 0.1f;

    //BUTTON COLOR
    [SerializeField] Color normalGraphicColor = Color.gray;
    [SerializeField] Color hoverGraphicColor = Color.white;

    //TEXT CONTROL
    [SerializeField] bool useText = false;
    public enum TextEffect { None, Fade}
    public TextEffect textEffect;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Color normalTextColor = Color.white;
    [SerializeField] Color hoverTextColor = Color.black;

    //EVENT
    [System.Serializable]
    public class OnHoverEvent : UnityEvent { }
    public class OnClickEvent : UnityEvent { }

    public OnHoverEvent onHoverEvent = new OnHoverEvent();
    public OnClickEvent onClickEvent = new OnClickEvent();

    #region getters and setters
    public RectTransform ButtonRect => buttonRect;
    public Vector2 ScaleIncreamental { get => scaleIncrement; set => scaleIncrement = value; }
    public float AnimationTime { get => buttonAnimationTime; set => buttonAnimationTime = value; }

    public Color NormalGraphicColor { get => normalGraphicColor; set => normalGraphicColor = value; }
    public Color HoverGraphicColor { get => hoverGraphicColor; set => hoverGraphicColor = value; }

    public bool UseText { get => useText; set => useText = value; }
    public TextMeshProUGUI Text { get => text; set => text = value; }
    public Color NormalTextColor { get => normalTextColor; set => normalTextColor = value; }
    public Color HoverTextColor { get => hoverTextColor; set => hoverTextColor = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        buttonRect = gameObject.GetComponent<RectTransform>();
        buttonGraphic = gameObject.GetComponent<Image>();

        buttonGraphic.color = normalGraphicColor;

        var useText = true ? text.color = normalTextColor : text.color = normalTextColor;

        buttonRectScale = buttonRect.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverEvent?.Invoke();
        switch (buttonAnimation)
        {
            case ButtonAnimation.Fade:
                buttonGraphic.DOColor(hoverGraphicColor, buttonAnimationTime);
                break;
            case ButtonAnimation.Move:
                break;
            case ButtonAnimation.Scale:
                buttonRect.DOSizeDelta(buttonRectScale + scaleIncrement, buttonAnimationTime, true);
                break;
            case ButtonAnimation.FadeAndMove:
                break;
            case ButtonAnimation.FadeAndScale:
                buttonRect.DOSizeDelta(buttonRectScale + scaleIncrement, buttonAnimationTime, true);
                buttonGraphic.DOColor(hoverGraphicColor, buttonAnimationTime);
                break;
            case ButtonAnimation.MoveAndScale:
                break;
            default:
                break;
        }

        switch (textEffect)
        {
            case TextEffect.None:
                break;
            case TextEffect.Fade:
                text.DOColor(hoverTextColor, buttonAnimationTime);
                break;
            default:
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (buttonAnimation)
        {
            case ButtonAnimation.Fade:
                buttonGraphic.DOColor(normalGraphicColor, buttonAnimationTime);
                break;
            case ButtonAnimation.Move:
                break;
            case ButtonAnimation.Scale:
                buttonRect.DOSizeDelta(buttonRectScale, buttonAnimationTime, true);
                break;
            case ButtonAnimation.FadeAndMove:
                break;
            case ButtonAnimation.FadeAndScale:
                buttonRect.DOSizeDelta(buttonRectScale, buttonAnimationTime, true);
                buttonGraphic.DOColor(normalGraphicColor, buttonAnimationTime);
                break;
            case ButtonAnimation.MoveAndScale:
                break;
            default:
                break;
        }

        switch (textEffect)
        {
            case TextEffect.None:
                break;
            case TextEffect.Fade:
                text.DOColor(normalTextColor, buttonAnimationTime);
                break;
            default:
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent?.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var useText = true ? textEffect = TextEffect.Fade : textEffect = TextEffect.None;
    }

}

public class OnClickEvent : UnityEvent { }