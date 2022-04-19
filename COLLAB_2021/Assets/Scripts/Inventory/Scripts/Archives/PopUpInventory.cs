using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInventory : MonoBehaviour
{
    public Canvas inventoryUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if ((Input.GetKeyDown(KeyCode.Return))||(Input.GetKeyDown(KeyCode.I)))
        {
            popUpInventory();
            Debug.Log("Hgfhfgh");
        }
        Debug.Log(GameManager.Instance.gameState);
    }
    public void popUpInventory()
    {
        if(inventoryUI.enabled==true)
        {
            GameManager.Instance.gameState = GameState.FreeRoam;

        }
        else
        if (inventoryUI.enabled == false)
        {
            GameManager.Instance.gameState = GameState.UpInventory;
        }

        inventoryUI.enabled = !inventoryUI.enabled;
        
    }
}
