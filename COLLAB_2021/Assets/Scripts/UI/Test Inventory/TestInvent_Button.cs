///Author: Phap Nguyen.
///Description: Control button interaction of the inventory UI.
///Day created: 27/11/2021
///Last edited: 29/11/2021 - Phap Nguyen.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TestInvent_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] float rotateAngle;
    [SerializeField] float rotateSpeed;
    //[SerializeField] AudioClip hoverAudio;
    Button button;

    [SerializeField] TestInvent_Panel inventoryPanel;
    [SerializeField] TestInvent_Item buttonItem;

    void Awake()
    {
        //Don't do this :(
        inventoryPanel = (TestInvent_Panel)FindObjectOfType(typeof(TestInvent_Panel));

        button = gameObject.GetComponent<Button>();
        button.transform.localRotation = Quaternion.identity;

        
    }

    /// <summary>
    /// Mouse Enter event.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.transform.DORotate(new Vector3(0, 0, rotateAngle), rotateSpeed);
    }

    /// <summary>
    /// Mouse Exit event.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        button.transform.DORotate(new Vector3(0, 0, 0), rotateSpeed);
    }

    /// <summary>
    /// Mouse Click event.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        button.Select();
        SelectButton();
    }

    public void SelectButton()
    {
        inventoryPanel.itemData = buttonItem;
        inventoryPanel.SetStatsSmooth();
    }
}
