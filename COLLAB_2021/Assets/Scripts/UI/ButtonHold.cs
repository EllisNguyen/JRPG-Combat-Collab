///Author: Phap Nguyen
///Description: Control the holding and releasing button.
///Day created: 27/03/2022
///Last edited: 27/03/2022 - Phap Nguyen.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;//TextMeshPro Lib

//Adding 2 Unity's EventSystems interface to control the interaction of the button.
public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    #region Variables
    float startTimer = 0;                                   //Start timer always 0.
    [Header("BUTTON HOLD TIMER")]
    [Range(0, 100)][SerializeField] float endTimer = 2;     //The end float that conclude the holding button.
    [SerializeField] float curTimer;                        //The float that will be increasing when holding button.
    [SerializeField] float incremental = 0.2f;              //The amount that will increase over time.
    [SerializeField] TextMeshProUGUI timerText;             //Text on the Button that change to reflex the current timer when holding the button.
    bool isHolding = false;

    [Space(20)]

    [Header("ADDITIONAL FLARES")]
    [SerializeField] bool useProgressBar;                   //Bool to check wether or not using the incrementFill image to indicate the holding progress.
    [SerializeField] Image incrementFill;                   //Ref to the progress image.

    [Space(20)]

    public UnityEvent holdFinish;                           //Event that will invoke when finish holding.
    public UnityEvent holdRelease;                          //Event that will invoke when finish holding.
    #endregion

    #region UNITY FUNCTION
    //Start the game with button hold timer = 0 to prevent increment or invoking any event unexpectedly.
    void Start()
    {
        ResetTimer();
    }

    //The Update() function have no use in the functionality of the script.
    //Only to display the text UI showcasing the progress of the holding.
    void Update()
    {
        if (curTimer == startTimer) timerText.text = "LMB to hold button.";
        else if (curTimer != startTimer && curTimer != endTimer) timerText.text = curTimer.ToString();
        else if(curTimer == endTimer) timerText.text = "Hold finished.";
    }
    #endregion

    #region Button holding process
    //Function run when hover and holding down the LMB.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Set bool to true.
        isHolding = true;

        //Init the coroutine HoldingButton().
        StartCoroutine(HoldingButton());
    }

    //Function run when the LMB is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        ResetTimer();

        holdRelease?.Invoke();
    }

    //Function run when the LMB is out of button hitbox.
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetTimer();

        holdRelease?.Invoke();
    }
    #endregion

    #region :)
    /// <summary>
    /// When holding down button, the timer will increase by incremental value (set in the editor) over time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator HoldingButton()
    {
        //While loop to keep running if curTimer is smaller than endTimer.
        while (curTimer < endTimer)
        {
            //Break out of the loop if the mouse input is up.
            if (!isHolding) break;

            //Increase the curTimer value.
            curTimer += incremental * Time.deltaTime;

            if (useProgressBar) incrementFill.fillAmount = curTimer / endTimer;

            //Invoke the UnityEvent when the holding timer reached the end.
            if (curTimer >= endTimer)
            {
                curTimer = endTimer;
                holdFinish?.Invoke();
            }

                yield return null;
        }

        //Set the incrementFill value.
        if (useProgressBar) incrementFill.fillAmount = curTimer / endTimer;
    }

    //Reset the button to the start position.
    void ResetTimer()
    {
        isHolding = false;
        curTimer = startTimer;
        incrementFill.fillAmount = curTimer;
    }
    #endregion
}
