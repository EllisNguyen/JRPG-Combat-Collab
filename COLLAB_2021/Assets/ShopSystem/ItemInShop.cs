using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ItemInShop : MonoBehaviour
{
    [SerializeField] List<BuyList> BuyList;
   [SerializeField] public GameObject BuyCanvas;
   //  [SerializeField] Text Description;
   // [SerializeField] Text ItemName;
   //  [SerializeField] Text BuyPrice;
   //  [SerializeField] Text SellPrice;
   //  [SerializeField] Image ItemImage;
    [SerializeField] ItemBase item;
    [SerializeField] SpriteRenderer graphic; 
    [SerializeField] GameObject keyGraphic;
    public buyCanvas _buyCanvas;
    int Money = 100000;
    // Start is called before the first frame update
    void Start()
    {
        _buyCanvas=BuyCanvas.GetComponent<buyCanvas>();
        //Description = GameObject.Find("Description").GetComponent<Text>();
        //ItemName = GameObject.Find("ItemName").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider player)
    {
        if (player.name == "Player")
        {
            keyGraphic.SetActive(true);
            BuyCanvas.SetActive(true);
            _buyCanvas.Description.text = BuyList[0].Item.ItemDescription;
            _buyCanvas.ItemName.text = BuyList[0].Item.ItemName;
            _buyCanvas.ItemImage.sprite = BuyList[0].Item.ItemSprite;
            _buyCanvas.BuyPrice.text = "Buy Price  "+BuyList[0].Item.BuyPrice.ToString();
            _buyCanvas.SellPrice.text = "Sell Price " + BuyList[0].Item.SellPrice.ToString();
          
        }
        
    }
    public void OnTriggerStay(Collider player)
    {
        var inventory = FindObjectOfType<PlayerEntity>().GetComponent<Inventory>();
        if (player.name == "Player")
        {
            keyGraphic.SetActive(true);
            BuyCanvas.SetActive(true);
            _buyCanvas.Description.text = BuyList[0].Item.ItemDescription;
            _buyCanvas.ItemName.text = BuyList[0].Item.ItemName;
            _buyCanvas.ItemImage.sprite = BuyList[0].Item.ItemSprite;
            _buyCanvas.BuyPrice.text = "Buy Price  "+BuyList[0].Item.BuyPrice.ToString();
            _buyCanvas.SellPrice.text = "Sell Price " + BuyList[0].Item.SellPrice.ToString();

            if (Input.GetKey(KeyCode.E))
            {
                if (Money < item.BuyPrice)
                {
                    Debug.Log("You Dont Have Enough Money to Buy");
                }
                if (Money > item.BuyPrice)
                {
                    inventory.AddItem(BuyList[0].Item,BuyList[0].ItemCount);
                    Debug.Log("Bought");
                    Money = Money - item.BuyPrice;
                    Destroy(this.gameObject);
                }
            }
        }   

    }
    public void OnTriggerExit(Collider player)
    {
        keyGraphic.SetActive(false);
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
