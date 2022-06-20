using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] Image titleImage;
    [SerializeField] Image introFade;
    [SerializeField] RectTransform titleRectTransform;
    [SerializeField] Vector2 titleCurrentLocation;
    [SerializeField] Vector2 titleNextLocationIncrement;
    [SerializeField] Vector2 titleCurrentSize;
    [SerializeField] Vector2 titleNextSizeIncrement;
    [SerializeField] float titleTimer;

    [Header("Other")]
    [SerializeField] AnimationCurve yScaleCurve;
    [SerializeField] CanvasGroup pressAnykey;
    [SerializeField] Image anykeyPressedAnim;
    [SerializeField] CanvasGroup menuGroup;
    public Ease EaseType;

    [SerializeField] float titleSequenceDuration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        titleCurrentLocation = titleRectTransform.anchoredPosition;
        titleCurrentSize = titleRectTransform.sizeDelta;
        StartCoroutine(TitleStartSequence());
        Destroy(GameObject.Find("PersistentObject"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(TitleTransitionSequence());
        }
    }

    public void HandleCurveAnim()
    {
        float blend = yScaleCurve.Evaluate(Time.time);
    }

    IEnumerator TitleStartSequence()
    {
        introFade.DOFade(0, titleSequenceDuration);
        titleImage.DOFade(1, titleSequenceDuration);
        yield return new WaitForSeconds(titleSequenceDuration / 2);
        pressAnykey.DOFade(1, titleSequenceDuration / 2);
    }

    IEnumerator TitleTransitionSequence()
    {
        var sequence = DOTween.Sequence();
        RectTransform animRect = anykeyPressedAnim.GetComponent<RectTransform>();

        sequence.Append(anykeyPressedAnim.DOFade(1, 0.075f).SetEase(this.EaseType));
        sequence.Join(animRect.DOSizeDelta(new Vector2(550, 1), 0.25f).SetEase(this.EaseType));
        sequence.Join(pressAnykey.DOFade(0, titleTimer / 3).SetEase(this.EaseType));
        sequence.Insert(0.08f, anykeyPressedAnim.DOFade(0, 0.075f).SetEase(this.EaseType));
        yield return sequence.WaitForCompletion();


        titleRectTransform.DOAnchorPos(titleCurrentLocation + titleNextLocationIncrement, titleTimer);
        titleRectTransform.DOSizeDelta(titleCurrentSize + titleNextSizeIncrement, titleTimer);
        //pressAnykey.gameObject.SetActive(false);
        anykeyPressedAnim.gameObject.SetActive(false);

        yield return new WaitForSeconds(titleTimer);
        menuGroup.DOFade(1, titleTimer);
    }

    void PressedAnyKey()
    {
        var sequence = DOTween.Sequence();
        RectTransform animRect = anykeyPressedAnim.GetComponent<RectTransform>();

        sequence.Append(anykeyPressedAnim.DOFade(1, 0.2f));
        sequence.Join(animRect.DOSizeDelta(new Vector2(200, 100), 0.2f));

        sequence.Append(anykeyPressedAnim.DOFade(0, 0.2f));
    }
}
