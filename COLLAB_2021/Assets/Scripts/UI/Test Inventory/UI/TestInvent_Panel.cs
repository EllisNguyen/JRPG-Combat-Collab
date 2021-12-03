///Author: Phap Nguyen.
///Description: Control the UI panel of the inventory: stat, progress bar, name, description stuff.
///Day created: 27/11/2021
///Last edited: 29/11/2021 - Phap Nguyen.

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

[TypeInfoBox("This is the scriptable object and also a parent class for all  type of items.")]
public class TestInvent_Panel : MonoBehaviour
{
    [HideInInspector] public TestInvent_Item itemData;

    public bool IsUpdatingInterface { get; private set; }

    #region struct stuff
    /// <summary>
    /// Get stat amount.
    /// </summary>
    [Serializable]
    public struct ItemStats
    {
        public int physAtk;
        public int physDef;
        public int specAtk;
        public int specDef;
        public int speed;
        public int critChance;
    }

    /// <summary>
    /// Get progress bar.
    /// </summary>
    [Serializable]
    public struct StatBar
    {
        public Image physAtk_Bar;
        public Image physDef_Bar;
        public Image specAtk_Bar;
        public Image specDef_Bar;
        public Image speed_Bar;
        public Image critChance_Bar;
    }

    /// <summary>
    /// Declare all stat number UI.
    /// </summary>
    [Serializable]
    public struct StatNumber
    {
        public TextMeshProUGUI physAtk_Num;
        public TextMeshProUGUI physDef_Num;
        public TextMeshProUGUI specAtk_Num;
        public TextMeshProUGUI specDef_Num;
        public TextMeshProUGUI speed_Num;
        public TextMeshProUGUI critChance_Num;
    }

    #endregion

    private ItemStats stats;

    [ShowInInspector, PropertySpace(16)]

    [Header("STAT VISUAL")]
    [InfoBox("Visual stuff for item stat.")]
    [SerializeField] private StatBar bar;

    [ShowInInspector, PropertySpace(16)]

    [InfoBox("Item display.")]
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] Image itemSprite;

    [InfoBox("Number text for stat progress bar.")]
    [SerializeField] private StatNumber numTxt;

    public void SetStatsSmooth()
    {
        //Do nothing if no data available.
        //Might need to do something if something is not available.
        if (itemData == null) return;

        //Set item name and description.
        itemName.text = itemData.NAME;
        itemDescription.text = itemData.DESCRIPTION;
        itemSprite.sprite = itemData.SPRITE;

        ////Get and set item stat value.
        //stats.physAtk = itemData.PHYS_ATK;
        //stats.physDef = itemData.PHYS_DEF;
        //stats.specAtk = itemData.SPEC_ATK;
        //stats.specDef = itemData.SPEC_DEF;
        //stats.speed = itemData.SPEED;
        //stats.critChance = itemData.CRIT_CHANCE;

        ////Animate the stat bar. Look cool :)
        //SmoothFillAmount(bar.physAtk_Bar, (float)itemData.PHYS_ATK / 100);
        //SmoothFillAmount(bar.physDef_Bar, (float)itemData.PHYS_DEF / 100);
        //SmoothFillAmount(bar.specAtk_Bar, (float)itemData.SPEC_ATK / 100);
        //SmoothFillAmount(bar.specDef_Bar, (float)itemData.SPEC_DEF / 100);
        //SmoothFillAmount(bar.speed_Bar, (float)itemData.SPEED / 100);
        //SmoothFillAmount(bar.critChance_Bar, (float)itemData.CRIT_CHANCE / 100);

        ////HARD CODE WARNING. (but these var are in constant number of var so writing code like this is fine)
        //numTxt.physAtk_Num.text = "+" + itemData.PHYS_ATK.ToString();
        //numTxt.physDef_Num.text = "+" + itemData.PHYS_DEF.ToString();
        //numTxt.specAtk_Num.text = "+" + itemData.SPEC_ATK.ToString();
        //numTxt.specDef_Num.text = "+" + itemData.SPEC_DEF.ToString();
        //numTxt.speed_Num.text = "+" + itemData.SPEED.ToString();
        //numTxt.critChance_Num.text = "+" + itemData.CRIT_CHANCE.ToString();
    }

    //Function that helps animate the fillamount image bar.
    public async void SmoothFillAmount(Image progressBar, float newVal)
    {
        progressBar.fillAmount = 0;

        while (progressBar.fillAmount < newVal)
        {
            progressBar.fillAmount += 2 * Time.deltaTime;
            await Task.Yield();
        }
        progressBar.fillAmount = newVal;
    }
}
