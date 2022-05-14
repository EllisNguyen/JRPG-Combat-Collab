///Author: Phap Nguyen.
///Description: About panel that show item information and stats.
///Day created: 20/01/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Unity UI lib.
using TMPro;//Text mesh pro, better text :)

public class ItemAboutPanel : MonoBehaviour
{
    #region VARIABLES
    public RectTransform rect;

    [Header("About")]
    [SerializeField] TextMeshProUGUI itemName;//Display name.
    [SerializeField] TextMeshProUGUI itemRarityTxt;//Display rarity (text).
    [SerializeField] Image rarityColorBar;//Rarity color bar.
    [SerializeField] Image rarityColorGradient;//Rarity gradient color.
    [SerializeField] TextMeshProUGUI itemDescription;//Display description.

    [Header("Core attribute control")]
    [SerializeField] CoreAttributeBar attributeBarPrefab;//Ref to the prefab that will display the attribute of the item.
    [SerializeField] GameObject coreAttributeContainer;//Ref to the GO that will contain all the attributes.
    [SerializeField] GameObject attributeSpawnObj;//Ref to the GO that will be the parent spawn of the attributes.
    [SerializeField] int attributeCount;//For display ref in editor only, to compare the number of attribute(s) with currently spawned.
    List<CoreAttributeBar> attributeBarList;//List of attributes.
    [SerializeField] RectTransform attributeRect;//Rect transform of the attribute container, to control the size when spawn attribute prefab(s).

    [Header("Panel FX")]
    [SerializeField] Image appearImg;

    [SerializeField] InventoryContainer inventoryContainer;

    //Expose the private variables.
    public int AttributeCount
    {
        get { return attributeCount; }
        set => attributeCount = value;
    }
    #endregion VARIABLES

    #region FUNCTIONS
    /// <summary>
    /// Call when pressed on an item in the inventory, setup the data of selected item on the button onto the about panel.
    /// </summary>
    /// <param name="selectedItem"></param>
    public void SetData(ItemBase selectedItem)
    {
        itemName.text = selectedItem.ItemName;
        itemRarityTxt.text = GetRarityFromItem(selectedItem).ToString();
        itemRarityTxt.color = selectedItem.GetRarityColor(selectedItem);
        itemDescription.text = selectedItem.ItemDescription;

        rarityColorBar.color = selectedItem.GetRarityColor(selectedItem);
        rarityColorGradient.color = selectedItem.GetRarityColor(selectedItem);

        //Display the attribute panel or not depend of the number of attribute(s).
        if (attributeCount > 0)
        {
            coreAttributeContainer.SetActive(true);
            AddCoreAttribute(selectedItem);
        }
        else coreAttributeContainer.SetActive(false);

        StartCoroutine(EnableAnim());
    }

    /// <summary>
    /// Function runs when showcasing an attributes with 1 or multiple attribute. Add and set the attribute info on the newly added prefabs.
    /// </summary>
    /// <param name="item"></param>
    public void AddCoreAttribute(ItemBase item)
    {
        //Clear all unrelated item in the attribute list.
        foreach (Transform child in attributeSpawnObj.transform)
        {
            //Destroy them all.
            Destroy(child.gameObject);
        }

        //Create a new attribute list.
        attributeBarList = new List<CoreAttributeBar>();

        //Search through all the attrute inside the list of attributes.
        foreach (var attribute in inventoryContainer.selectedItem.GetCoreAttributes(item))
        {
            //Declare a variable as the spawned attributes.
            CoreAttributeBar attributeUIObj = Instantiate(attributeBarPrefab, attributeSpawnObj.transform);

            //Update the information inside the attribute class.
            attributeUIObj.SetAttributeInfo(attribute);

            //Add the attributes to list.
            attributeBarList.Add(attributeUIObj);
        }

        //Update the size of the attribute holder to fit all the newly added attributes.
        //New size only change in the y, which is (attribute label size + (attributePrefab.y * number of spawned prefab)). 
        attributeRect.sizeDelta = new Vector2(attributeRect.sizeDelta.x, 20 + (20 * attributeCount));
    }

    /// <summary>
    /// Clear data on the about panel, including name, rarity color and activate the image that close off the panel.
    /// </summary>
    public void ClearData()
    {
        itemName.text = "";
        itemRarityTxt.text = "";

        ResetAppearAnim();
    }

    /// <summary>
    /// Reset the fill amount of the appear image overlay to 1 (fully cover the about panel).
    /// </summary>
    public void ResetAppearAnim()
    {
        appearImg.fillAmount = 1;
    }

    /// <summary>
    /// Procedurally animate the fill amount of the appear image overlay from 1 to 0, to uncover the about panel everytime a new item is selected.
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnableAnim()
    {
        //Set the fill amount back to 1.
        appearImg.fillAmount = 1;

        //While loop that keep on running when condition is not met.
        //Filling the image's fillamount overtime.
        while (appearImg.fillAmount > 0)
        {
            appearImg.fillAmount -= 4f * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// Get the type of rarity off the selected item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    ItemRarity GetRarityFromItem(ItemBase item)
    {
        if (item.rarity is ItemRarity.Common)
            return ItemRarity.Common;
        else if (item.rarity is ItemRarity.Uncommon)
            return ItemRarity.Uncommon;
        else if (item.rarity is ItemRarity.Rare)
            return ItemRarity.Rare;
        else if (item.rarity is ItemRarity.Legendary)
            return ItemRarity.Legendary;
        else
            return ItemRarity.Exotic;
    }
    #endregion FUNCTIONS
}
