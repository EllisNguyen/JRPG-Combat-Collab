///Author: Phap Nguyen.
///Description: Container of the Inventory, control the spawn of inventory object and UI.
///Day created: 22/03/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;//TextMeshPro lib.
using DG.Tweening;//DOTween lib.

//Requirement of the CanvasGroup helps with the effect of transition in and out of the invencotyr screen.
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AnimatedUi))]
public class ItemPanel : MonoBehaviour
{
    [SerializeField] [HideInInspector] ItemBase _item;//ItemBase ref to get const data from the preset item.
    [SerializeField] int inventoryPosition;//Getting the current position of item in the inventory (depricated).

    [Header("Item display")]
    [SerializeField] Image itemImage;//Item icon.
    [SerializeField] Image rarityColorBar;//Rarity color bar.
    [SerializeField] Image rarityGradient;//Gradient of the rarity color.

    [Header("Item information")]
    [SerializeField] TextMeshProUGUI itemName;//UI show the item name.
    [SerializeField] TextMeshProUGUI itemPrice;//UI show the item price (only in shop).

    [Header("Interaction display")]
    [SerializeField] Image equippedIndicator;//Icon to indicate player that item is equipped.
    [SerializeField] bool equipped = false;
    [SerializeField] GameObject stackIndicator;//Object that indicate player the item is stackable + the number of current stack.
    [SerializeField] TextMeshProUGUI stackNumber;//Number of current stack.

    ItemAboutPanel itemAboutPanel;//Ref to the item description panel.
    [SerializeField] CanvasGroup canvasGroup;//Ref to the CanvasGroup component.
    [SerializeField] float fadeInTime = 0.1f;//Fade in and out timer for the item panel.
    [SerializeField] AnimatedUi uiComponent;

    InventoryContainer inventoryContainer;

    //Expose the private variables.
    public CanvasGroup CanvasGroup => canvasGroup;
    public AnimatedUi UIComponent => uiComponent;
    public ItemBase Item => _item;
    public int InventoryPosition
    {
        get { return inventoryPosition; }
        set { inventoryPosition = value; }
    }

    //Getting the item description panel from the GameManager.
    //Set current alpha for the CanvasGroup to 0 -> to init the fadein FX on spawn.
    public void Init()
    {
        itemAboutPanel = GameManager.Instance.itemAbout;
        inventoryContainer = GameManager.Instance.inventoryContainer;

        canvasGroup.alpha = 0;
    }

    public void HandleUpdate()
    {
        uiComponent.HandleCurveAnim();
    }

    //Do the fadein anim.
    public IEnumerator EnableInventory()
    {
        canvasGroup.DOFade(1, fadeInTime);
        yield return new WaitForSeconds(fadeInTime);
    }

    /// <summary>
    /// Call upon the SetData function.
    /// Apply the data onto the UI.
    /// </summary>
    public void SetAboutUI()
    {
        inventoryContainer.selectedItem = CurrentSelectedPanel();
        itemAboutPanel.AttributeCount = inventoryContainer.selectedItem.Item.CoreAttributes.Count;
        itemAboutPanel.SetData(_item);
        inventoryContainer.UpdateSelection();
    }

    /// <summary>
    /// Clear all item information on the about panel, and nullify the reference to any previously selected item.
    /// </summary>
    public void ClearAboutUI()
    {
        itemAboutPanel.ClearData();
        inventoryContainer.selectedItem = null;
    }

    /// <summary>
    /// Update the animating aspect of the selected UI.
    /// </summary>
    public void UpdateSelection()
    {
        uiComponent.UiSelected();
        if (inventoryContainer.selectedItem != this) uiComponent.UiDeselected();
    }

    /// <summary>
    /// Return selected item panel as self.
    /// </summary>
    /// <returns></returns>
    ItemPanel CurrentSelectedPanel()
    {
        return this;
    }

    /// <summary>
    /// Get the data of the item from the ItemSlot class which return the ItemBase class and int of the stack number.
    /// Then set the data onto the requires UI element(s).
    /// </summary>
    /// <param name="itemSlot"></param>
    public void SetData(ItemSlot itemSlot)
    {
        //Set the current ItemBase to be the param ItemBase.
        _item = itemSlot.Item;

        //Setup the rarity color on the UI.
        switch (itemSlot.Item.rarity)
        {
            case ItemRarity.Common:
                rarityColorBar.color = GameManager.Instance.common;
                rarityGradient.color = GameManager.Instance.common;
                break;
            case ItemRarity.Uncommon:
                rarityColorBar.color = GameManager.Instance.uncommon;
                rarityGradient.color = GameManager.Instance.uncommon;
                break;
            case ItemRarity.Rare:
                rarityColorBar.color = GameManager.Instance.rare;
                rarityGradient.color = GameManager.Instance.rare;
                break;
            case ItemRarity.Legendary:
                rarityColorBar.color = GameManager.Instance.legendary;
                rarityGradient.color = GameManager.Instance.legendary;
                break;
            case ItemRarity.Exotic:
                rarityColorBar.color = GameManager.Instance.exotic;
                rarityGradient.color = GameManager.Instance.exotic;
                break;
            default:
                break;
        }

        //Apply UI element(s).
        itemImage.sprite = itemSlot.Item.ItemSprite;
        itemName.text = itemSlot.Item.ItemName;
        itemPrice.text = itemSlot.Item.SellPrice.ToString();

        //Check for item stackable bool.
        //Then enable or disable the stacking component/UI accordingly.
        if (itemSlot.Item.Stackable)
        {
            stackIndicator.SetActive(true);
            stackNumber.text = itemSlot.Count.ToString();
        }
        else stackIndicator.SetActive(false);

        //Same as for stacking but this one is for checking equipped weapon.
        if (!equipped) equippedIndicator.gameObject.SetActive(false);
        else equippedIndicator.gameObject.SetActive(true);
    }

    public List<CoreAttribute> GetCoreAttributes(ItemBase item)
    {
        item = _item;
        return item.CoreAttributes;
    }

    public int CoreAttributeCount()
    {
        int count = GetCoreAttributes(_item).Count;
        return count;
    }
}
