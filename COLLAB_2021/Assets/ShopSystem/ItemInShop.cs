using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInShop : MonoBehaviour
{
    [SerializeField] List<BuyList> BuyList;
    [SerializeField] GameObject BuyCanvas;
    [SerializeField] Text Description;

    // Start is called before the first frame update
    void Start()
    {
        //Description.GetComponent<Text>.name
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void OnTriggerEnter(Collider player)
    //{
    //    if (player.name == "Player")
    //    {
    //        BuyCanvas.SetActive(true);
    //        Description.text=
    //    }
    //}

}

public class BuyList
{
    [SerializeField] ItemBase item;
    [Range(1,5)][SerializeField] int itemCount;

    //Publicly expose the local variables.
    public ItemBase Item => item;
    public int ItemCount => itemCount;
}
