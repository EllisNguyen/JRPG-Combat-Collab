///Author: Phap Nguyen.
///Description: Script that control the animated aspect of the UI.
///Day created: 27/11/2021
///Last edited: 28/11/2021 - Phap Nguyen.

//////////////////////////////////////////////////////////////////////
/// 
///PLEASE DON'T TOUCH THIS CLASS IF YOU DON'T HAVE TO.
///AND PLEASE CONTACT PHAP IF ANY ERROR(S) LEAD TO THIS CLASS.
///
///YOU FIND NO EXPLANATIONS OR COMMENTS IN THIS CLASS FOR A REASON.
///
//////////////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;


public class AnimatedUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool ignoreMouse;
    [SerializeField] bool staySelected = false;
    bool isSelected;
    [SerializeField] RectTransform rect;
    public Vector2 fromPosition;
    public Vector2 toPosition;
    public float animateTime;

    [Header("Selected flare")]
    [SerializeField] bool blinkOutline;
    [SerializeField] AnimationCurve blinkCurve;
    [SerializeField] float blinkTime;
    [SerializeField] Image outline;
    float startOutline;

    Vector3 originPos;
    [SerializeField] Vector3 moveToPos;

    public UnityEvent onMouseOver;
    public UnityEvent onClick;

    private void Start()
    {

        if (rect == null) rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = fromPosition;
        isSelected = false;
        if (blinkOutline)
        {
            startOutline = outline.pixelsPerUnitMultiplier;
            blinkCurve.postWrapMode = WrapMode.PingPong;
        }
    }

    public void HandleCurveAnim()
    {
        float blend = blinkCurve.Evaluate(Time.time);
        outline.pixelsPerUnitMultiplier = blend;
    }

    public void Move()
    {
        StartCoroutine(AnimateMove());
    }

    IEnumerator AnimateMove()
    {
        //rect.DOLocalMove(toPosition, animateTime);
        rect.DOAnchorPos(toPosition, animateTime);

        yield return new WaitForSeconds(animateTime);

        rect.anchoredPosition = fromPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ignoreMouse) return;

        onMouseOver?.Invoke();
        rect.DOAnchorPos3D(moveToPos, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ignoreMouse) return;
        if (isSelected && staySelected) return;

        rect.DOAnchorPos3D(originPos, 0.1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ignoreMouse) return;
        onClick?.Invoke();
    }

    public void UiSelected()
    {
        isSelected = true;
        rect.DOAnchorPos3D(moveToPos, 0.1f);

        if (blinkOutline) HandleCurveAnim();
    }

    public void UiDeselected()
    {
        isSelected = false;
        if(blinkOutline) outline.pixelsPerUnitMultiplier = startOutline;
        rect.DOAnchorPos3D(originPos, 0.1f);
    }

    public bool Selected()
    {
        return isSelected;
    }
}
