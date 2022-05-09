using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ItemInShop : MonoBehaviour
{
    [SerializeField] List<BuyList> BuyList;
   [SerializeField] GameObject BuyCanvas;
    [SerializeField] Text Description;
   [SerializeField] Text ItemName;
    [SerializeField] Text BuyPrice;
    [SerializeField] Text SellPrice;
    [SerializeField] Image ItemImage;
    [SerializeField] ItemBase item;
    [SerializeField] SpriteRenderer graphic;
    // Start is called before the first frame update
    void Start()
    {
 
       Description = GameObject.Find("Description").GetComponent<Text>();
       ItemName = GameObject.Find("ItemName").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider player)
    {
        if (player.name == "Player")
        {
            BuyCanvas.SetActive(true);
            Description.text = BuyList[0].Item.ItemDescription;
            ItemName.text = BuyList[0].Item.ItemName;
            ItemImage.sprite = BuyList[0].Item.ItemSprite;
            BuyPrice.text = "Buy Price  "+BuyList[0].Item.BuyPrice.ToString();
            SellPrice.text = "Sell Price " + BuyList[0].Item.SellPrice.ToString();
        }
    }
    public void OnTriggerExit(Collider player)
    {
        
            BuyCanvas.SetActive(false);
           

        
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        graphic = GetComponentInChildren<SpriteRenderer>();
        if (BuyList.Count == 0)
        {
            graphic.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Assets/UI/pickup_null.png", typeof(Sprite));
            return;
        }

        if (BuyList.Count > 1)
        {
            graphic.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Assets/UI/pickup_pouch.png", typeof(Sprite));
        }
        else if (BuyList.Count == 1)
        {
            graphic.sprite = BuyList[0].Item.ItemSprite;
        }
    }
#endif
}


[System.Serializable]
public class BuyList
{
    [SerializeField] ItemBase item;
    [Range(1,5)][SerializeField] int itemCount;

    //Publicly expose the local variables.
    public ItemBase Item => item;
    public int ItemCount => itemCount;
}
