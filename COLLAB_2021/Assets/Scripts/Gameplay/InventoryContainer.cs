///Author: Phap Nguyen.
///Description: Container of the Inventory, control the spawn of inventory object and UI.
///Day created: 20/01/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Collections.Generic;
using UnityEngine;
using TMPro;//Better text :)
using DG.Tweening;//Procedural animation addon lib.

public class InventoryContainer : MonoBehaviour
{
    List<ItemPanel> itemPanelList;
    [SerializeField] GameObject itemList;//Ref to the item list grid container.
    [SerializeField] ItemPanel itemPanel;//Ref to the prefab contain ItemPanel class.
    [SerializeField] TextMeshProUGUI capacityText;//Text element of the capacity.

    RectTransform itemListRectTransform;
    Inventory inventory;
    int selectedCategory = 0;

    public ItemPanel selectedItem;
    ItemPanel prevSelectedItem;
    ItemAboutPanel aboutPanel;

    [Header("Indicators")]
    [SerializeField] GameObject emptyNotice;

    //Expose the private variables.
    public GameObject EmptyNotice
    {
        get { return emptyNotice; }
        set { emptyNotice = value; }
    }

    //Run OnStart().
    void OnEnable()
    {
        OnStart();
    }

    //Show list of item in the UI.
    private void Start()
    {
        OnStart();
        aboutPanel = GameManager.Instance.itemAbout;
        inventory.OnUpdated += UpdateItemList;

        prevSelectedItem = selectedItem;
    }

    /// <summary>
    /// Collective function that should be running when the game start and when (re)open the inventory.
    /// Reset the category back to the first one + udpate the list of item (in case picking up a new item when inventory is off).
    /// </summary>
    void OnStart()
    {
        ChangeItemCategory(0);
        UpdateItemList();
    }

    //Get Inventory class and the rect transform of the container grid.
    private void Awake()
    {
        //Get the inventory list.
        inventory = Inventory.GetInventory();

        itemListRectTransform = itemList.GetComponent<RectTransform>();
    }

    //Check for selected item, and only run the active animation for the on selected.
    public void Update()
    {
        if (itemPanelList.Count == 0) return;

        if (!selectedItem) return;
            
        selectedItem.HandleUpdate();
    }

    /// <summary>
    /// Update list of item in the inventory find and destroy all current child prefab in the item grid layout,
    /// then add the new list of items getting from the Inventory class.
    /// </summary>
    void UpdateItemList()
    {
        //Clear all unrelated item in the item list.
        foreach (Transform child in itemList.transform)
        {
            //Destroy them all.
            Destroy(child.gameObject);
        }

        //Initialize the list of ItemPanel.
        itemPanelList = new List<ItemPanel>();

        //Create the SlotUI prefab and assign correct information.
        //Instantiate :)
        foreach (var itemSlot in inventory.GetSlotsByCategory(selectedCategory))
        {
            var slotUIObj = Instantiate(itemPanel, itemList.transform);
            slotUIObj.Init();
            slotUIObj.SetData(itemSlot);

            //Add all instantiated pref into the slot UI list.
            itemPanelList.Add(slotUIObj);

            StartCoroutine(slotUIObj.EnableInventory());

        }

        for (int i = 0; i < itemPanelList.Count; i++)
        {
            selectedItem = itemPanelList[0];
            selectedItem.SetAboutUI();

            itemPanelList[i].name = $"Item {i} - {itemPanelList[i].Item.name}";
        }

        //Show capacity number and color indication.
        capacityText.text = $"Capacity: {inventory.GetTotalItemNumber()} / {GameManager.Instance.inventoryCapacity}";
        if(inventory.GetTotalItemNumber() < GameManager.Instance.inventoryCapacity)
            capacityText.color = GameManager.Instance.belowCapacity;
        else
            capacityText.DOColor(GameManager.Instance.aboveCapacity, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// Update the selected UI in the inventory menu.
    /// </summary>
    public void UpdateSelection()
    {
        selectedItem.UpdateSelection();

        prevSelectedItem?.UpdateSelection();
        prevSelectedItem = selectedItem;

        if (selectedItem == null) return;
    }

    /// <summary>
    /// Function run on selecting category.
    /// Called upon OnClick() in Unity's button.
    /// </summary>
    /// <param name="index"></param>
    public void ChangeItemCategory(int index)
    {
        selectedCategory = index;

        if (inventory.GetItemInSlot(selectedCategory) == 0)
        {
            selectedItem.ClearAboutUI();
            selectedItem = null;
            prevSelectedItem = null;

            //Clear all unrelated item in the item list.
            foreach (Transform child in itemList.transform)
            {
                //Destroy them all.
                Destroy(child.gameObject);
            }
            emptyNotice.SetActive(true);

            return;
        }
        UpdateItemList();
        emptyNotice.SetActive(false);
    }
}
