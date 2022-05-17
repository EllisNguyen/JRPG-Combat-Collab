using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buyCanvas : MonoBehaviour
{
    [SerializeField] public Text Description;
    [SerializeField]public Text ItemName;
    [SerializeField] public Text BuyPrice;
    [SerializeField] public Text SellPrice;
    [SerializeField] public Image ItemImage;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
