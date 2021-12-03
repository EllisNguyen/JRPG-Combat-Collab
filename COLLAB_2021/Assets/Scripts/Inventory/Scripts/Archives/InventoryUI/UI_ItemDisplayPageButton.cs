using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

///Author: Huynh Vu Long Hai
///Description: Changes the page number on click
///Day created: 09/10/2021
///Last edited: 09/10/2021 - Hai


public class UI_ItemDisplayPageButton : MonoBehaviour, IPointerClickHandler
{
    public UI_ItemDisplayManager displayManager;

    public bool isIncrement;
    private string pageType;
    public TMPro.TextMeshProUGUI pageTypeText;



    public void OnPointerClick(PointerEventData eventData)
    {
        int i = 0;

        if (isIncrement)
        {
            i = 1;
            PageTypeText();
            pageTypeText.text = pageType;
        }
        else
        {
            i = -1;
            PageTypeText();
            pageTypeText.text = pageType;
        }

        displayManager.pageNumber += i;

        if (displayManager.pageNumber > 5)
        {
            displayManager.pageNumber = 0;
        }
        else if (displayManager.pageNumber < 0)
        {
            displayManager.pageNumber = 5;
        }

        displayManager.UpdateInventoryDisplay(displayManager.inventoryToDisplay);

    }
  public  void PageTypeText()
    {

        switch (displayManager.pageNumber)
        {
            case 5:
                pageType = "other";
                return;
            case 4:
                pageType = "Key";
                return;
            case 3:
                pageType = "Armor";
                return;
            case 2:
                pageType = "Weapon";
                return;
            case 1:
                pageType = "Stats";
                return;
            default:

                pageType = "Health1";
                return;


        }

    }
}
