using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

///Author: Huynh Vu Long Hai
///Description: Displays an item based on the info passed in. Will be instantiated by the inventory manager when the player goes in the inventory.
///Day created: 13/10/2021
///Last edited: 23/10/2021 - Hai

public class UI_ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UI_ItemDisplayManager displayManager;

    public InventorySlot inventorySlot; //item from inventory to display

    ItemObject item;

    private Sprite itemSprite;
    private string itemName;
    private int itemStack;
    private string itemDesc;
    private ItemType itemType;

    private int itemValue;

    private int[] itemAttributes;

    public GameObject itemImage;

    [Tooltip("The text object that displays the stack.")]
    public TMPro.TextMeshProUGUI stackText;

    public TMPro.TextMeshProUGUI nameText; //to show the name of the item

    public TMPro.TextMeshProUGUI typeText; //to show the type of the item

    [Tooltip("The text object that displays the description.")]
    public TMPro.TextMeshProUGUI descText;

    [Tooltip("The image object that displays the sprite.")]
    public GameObject spriteImage;

    
    /// <summary>
    /// Initialize the data for the item
    /// </summary>
    public void Init()
    {
        item = inventorySlot.item;

        itemSprite = item.itemSprite;
        itemName = item.name;
        itemStack = inventorySlot.stack;
        itemDesc = item.itemDescription;
        itemType = item.itemType;

        itemValue = item.value;

        itemAttributes = item.atttributes;

        Display();
    }

    /// <summary>
    /// Set up the info to display
    /// </summary>
    void Display()
    {
        spriteImage.GetComponent<Image>().sprite = itemSprite;

        stackText.text = "" + itemStack;
    }

    //sets the description to show
    string SetDescription()
    {
        string descToReturn = null;

        switch (itemType)
        {
            case ItemType.Health:
                string extra = null;

                if (itemAttributes[1] > 0)
                {
                    extra = "Heal Duration: " + itemAttributes[1];
                }

                descToReturn = "Heal Value: " + itemAttributes[0] + "\n" + extra;

                break;
            case ItemType.Stats:
                descToReturn = "Health Modifier: " + itemAttributes[0] + "\n" +
                               "Mana Modifier: " + itemAttributes[1] + "\n" +
                               "P.Attack Modifier: " + itemAttributes[2] + "\n" +
                               "S.Attack Modifier: " + itemAttributes[3] + "\n" +
                               "P.Defense Modifier: " + itemAttributes[4] + "\n" +
                               "S.Defense Modifier: " + itemAttributes[5] + "\n" +
                               "Speed Modifier: " + itemAttributes[6] + "\n" +
                               "Crit Chance Modifier: " + itemAttributes[7] + "\n" +
                               "Crit Damage Modifier: " + itemAttributes[8] + "\n" +
                               "Duration: " + itemAttributes[9] + " seconds";

                break;
            case ItemType.Weapon:
                descToReturn = "Health Modifier: " + itemAttributes[0] + "\n" +
                               "Mana Modifier: " + itemAttributes[1] + "\n" +
                               "P.Attack Modifier: " + itemAttributes[2] + "\n" +
                               "S.Attack Modifier: " + itemAttributes[3] + "\n" +
                               "P.Defense Modifier: " + itemAttributes[4] + "\n" +
                               "S.Defense Modifier: " + itemAttributes[5] + "\n" +
                               "Speed Modifier: " + itemAttributes[6] + "\n" +
                               "Crit Chance Modifier: " + itemAttributes[7] + "\n" +
                               "Crit Damage Modifier: " + itemAttributes[8] + "\n" +
                               "Element: " + GetElement(itemAttributes[9]);

                break;
            case ItemType.Armor:
                descToReturn = "Health Modifier: " + itemAttributes[0] + "\n" +
                               "Mana Modifier: " + itemAttributes[1] + "\n" +
                               "P.Attack Modifier: " + itemAttributes[2] + "\n" +
                               "S.Attack Modifier: " + itemAttributes[3] + "\n" +
                               "P.Defense Modifier: " + itemAttributes[4] + "\n" +
                               "S.Defense Modifier: " + itemAttributes[5] + "\n" +
                               "Speed Modifier: " + itemAttributes[6] + "\n" +
                               "Crit Chance Modifier: " + itemAttributes[7] + "\n" +
                               "Crit Damage Modifier: " + itemAttributes[8] + "\n" +
                               "Element: " + GetElement(itemAttributes[9]);

                break;
            default:
                break;
        }

        return descToReturn;
    }

    //gets the element of the item based on the int passed in
    string GetElement(int elementIndex)
    {
        string element = null;

        switch (elementIndex)
        {
            case 0:
                element = "Heat";
                
                break;
            case 1:
                element = "Electric";

                break;
            case 2:
                element = "Radiation";

                break;
            case 3:
                element = "Ice";

                break;
            case 4:
                element = "Light";

                break;
            case 5:
                element = "Dark";

                break;
            default:
                element = "None";

                break;
        }

        return element;
    }

    string GetItemType(ItemType type)
    {
        string typeString = null;

        typeString = type.ToString();

        return typeString;
    }

    /// <summary>
    /// Updates description when the player hovers their mouse over, will implement a switch to alternate the description based on item's type
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemImage.GetComponent<Image>().sprite = itemSprite;

        nameText.text = itemName;

        descText.text = itemDesc;

        typeText.text = GetItemType(itemType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemImage.GetComponent<Image>().sprite = null;

        nameText.text = null;

        descText.text = null;

        typeText.text = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        displayManager.ReturnClickedItem(inventorySlot);
        item.Use();
    }
    
        
        

}
