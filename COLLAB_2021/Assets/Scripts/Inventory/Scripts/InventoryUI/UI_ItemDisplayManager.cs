using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Huynh Vu Long Hai
/// Desc: Manages the spawning of item to display. Handles updating their description. Returns the selection of items.
/// Created on: 19/10/2021
/// Last modified: 23/10/2021 - Hai
/// </summary>

public class UI_ItemDisplayManager : MonoBehaviour
{
    public InventoryScriptableObject inventory;

    public List<InventorySlot> inventoryToDisplay = new List<InventorySlot>();
    List<UI_ItemDisplay> displays = new List<UI_ItemDisplay>();

    public int pageNumber; //to determine what type of item should be shown

    public GameObject itemImage;
    public GameObject displayPrefab;
    public TMPro.TextMeshProUGUI nameText, descriptionText, typeText, pageTypeText; //to display the name, description, and type of the item
    public GameObject displayPanel;




    public InventorySlot clickedItem; //clicked item to return to the inventory manager
    ItemObject Item;

    public void OnEnable()
    {
        Debug.Log("Here");

        inventoryToDisplay = inventory.inventory;

        UpdateInventoryDisplay(inventoryToDisplay);

       
    }

    /// <summary>
    /// Clears the existing displays, updates the display list and sets up the displays
    /// </summary>
    /// <param name="newInventory"></param>
    public void UpdateInventoryDisplay(List<InventorySlot> newInventory)
    {
        ClearDisplay(displays);

        inventoryToDisplay = newInventory;

        DisplayInventory(inventoryToDisplay, pageNumber);
       
    }

    /// <summary>
    /// Calls SetUpDisplay() to set up the displays, then orders them
    /// </summary>
    /// <param name="inventorySlots"></param>
    void DisplayInventory(List<InventorySlot> inventorySlots, int type = 0)
    {
        SetUpDisplay(inventorySlots, type);

    }

    /// <summary>
    /// Set up the newly spawned item display
    /// </summary>
    /// <param name="itemToDisplay"></param>
    void SetUpDisplay(InventorySlot itemToDisplay, int type = 0)
    {
        if (type == ((int)itemToDisplay.item.itemType))
        {
            var spawnedDisplay = Instantiate(displayPrefab, displayPanel.transform);

            var newDisplay = spawnedDisplay.GetComponent<UI_ItemDisplay>();
           
            newDisplay.inventorySlot = itemToDisplay;
            newDisplay.itemImage = itemImage;
            newDisplay.nameText = nameText;
            newDisplay.descText = descriptionText;
            newDisplay.typeText = typeText;
            newDisplay.displayManager = this.GetComponent<UI_ItemDisplayManager>();

            newDisplay.Init();

            displays.Add(newDisplay);
            var newDisplaypage = spawnedDisplay.GetComponent<UI_ItemDisplayPageButton>();
            newDisplaypage.pageTypeText = pageTypeText;

            Debug.Log(nameText);
        }
    }

    /// <summary>
    /// Overloaded SetUpDisplay(), for convenience's sake
    /// </summary>
    /// <param name="itemsToDisplay"></param>
    void SetUpDisplay(List<InventorySlot> itemsToDisplay, int type = 0)
    {
        foreach (InventorySlot slot in itemsToDisplay)
        {
            SetUpDisplay(slot, type);
        }
    }

    /// <summary>
    /// Clear the displays passed in
    /// </summary>
    /// <param name="displaysToClear"></param>
    void ClearDisplay(List<UI_ItemDisplay> displaysToClear)
    {
        foreach (UI_ItemDisplay display in displaysToClear)
        {
            if (display)
            {
                Destroy(display.gameObject);
            }
        }

        displays.Clear();
    }

    public void ReturnClickedItem(InventorySlot item)
    {
        clickedItem = item;


    }
 
}


    

