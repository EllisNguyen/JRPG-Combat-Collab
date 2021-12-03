///Author: Phap Nguyen.
///Description: Control button interaction of the inventory UI.
///Day created: 27/11/2021
///Last edited: 29/11/2021 - Phap Nguyen.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TestInvent_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] float rotateAngle;
    [SerializeField] float rotateSpeed;
    //[SerializeField] AudioClip hoverAudio;
    Button button;

    [SerializeField] TestInvent_Panel inventoryPanel;
    [SerializeField] TestInvent_Item buttonItem;
    [SerializeField] TestInventory_Ui baseInventory;

    [SerializeField] Image itemSprite;
    [SerializeField] TextMeshProUGUI countText;

    void Awake()
    {
        // :(
        inventoryPanel = (TestInvent_Panel)FindObjectOfType(typeof(TestInvent_Panel));
        baseInventory = (TestInventory_Ui)FindObjectOfType(typeof(TestInventory_Ui));

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
        //button.Select();
        SelectButton();
    }

    public void SelectButton()
    {
        inventoryPanel.itemData = buttonItem;
        inventoryPanel.SetStatsSmooth();
        //baseInventory.HandleUpdate();
    }

    public void SetData(ItemSlott itemSlot)
    {
        itemSprite.sprite = itemSlot.ITEM.SPRITE;
        countText.text = $"x{itemSlot.COUNT}";
    }
}
