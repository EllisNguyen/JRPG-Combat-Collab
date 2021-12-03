using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInventory_Ui : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] TestInvent_Button itemSlotUI;

    [SerializeField] int selectedItem;
    List<TestInvent_Button> slotUIList;
    TestInventory inventory;

    [SerializeField] Color highlight;
    [SerializeField] Color nonHighlight;

    void Awake()
    {
        //WTF
        inventory = TestInventory.GetInventory();
    }

    void Start()
    {
        UpdateItem();
    }

    void UpdateItem()
    {
        //Clear current items.
        foreach (Transform child in itemList.transform) Destroy(child.gameObject);

        slotUIList = new List<TestInvent_Button>();
        foreach (var itemSlot in inventory.SLOTS)
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }
    }

    public void HandleUpdate()
    {
        int prevSelection = selectedItem;
        UpdateSelection();
        //if (Input.GetKeyDown(KeyCode.X)) onBack?.Invoke();
    }

    public void UpdateSelection()
    {
        

        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
            {
                slotUIList[i].GetComponent<Image>().color = highlight;
                print("selected " + i);
            }
            else
            {
                slotUIList[i].GetComponent<Image>().color = nonHighlight;
                print("selected " + i);
            }
        }
    }
}
