///Author: Phab Nguyen
///Description: Stat storage and calculation.
///Day created: 05/11/2021
///Last edited: 17/11/2021 - Phab Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{

    public List<TabButton> tabButtons;
    
    [Header("Button action color")]
    public Color idleColor;//Color when the tab button is not-selected
    public Color hoverColor;//Color when the mouse is not on the tab button
    public Color activeColor;//Color when the tab button is selected
    
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);

        OnTabSelected(tabButtons[0]);
    }

    //Exec when the mouse enter the tab button
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();//Reset tab image's color to normal
        if(selectedTab == null || button != selectedTab)
        {
            button.background.color = hoverColor;//Setup the correct color
        }
    }

    //Exec when the mouse is not on the tab button
    public void OnTabExit(TabButton button)
    {
        ResetTabs();//Reset tab image's color to normal
    }

    //Exec when the tab is clicked on, selected
    public void OnTabSelected(TabButton button)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        selectedTab = button;
        ResetTabs();//Reset tab image's color to normal
        //button.background.color = activeColor;//Setup the correct color

        int index = button.transform.GetSiblingIndex();//get the index of the child object in the group of tab buttons
        //Loop through the list of child object
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            //Check if 
            if(i == index)
            {
                //Enable the tab component
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                //Disable the tab component
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            //Skip through the reset if one of the tab is selected
            if (selectedTab != null && button == selectedTab) continue;
            //button.background.color = idleColor;
        }
    }

}
