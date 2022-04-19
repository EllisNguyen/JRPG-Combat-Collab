using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public GameObject inventory;
    public GameObject party;
    GameObject curMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ActivateMenu(inventory);
        }

        if (Input.GetKeyDown(KeyCode.U)) 
        {
            ActivateMenu(party);
        }

    }

    /// <summary>
    /// Menu activation functionality.
    /// Enable menu if not currently active + set correct gamestate and vice versa.
    /// </summary>
    /// <param name="menu"></param>
    void ActivateMenu(GameObject menu)
    {
        if(curMenu == menu)
        {
            menu.SetActive(false);
            curMenu = null;
            GameManager.Instance.gameState = GameState.FreeRoam;
        }
        else
        {
            if(curMenu != null)
                curMenu.SetActive(false);

            curMenu = menu;
            menu.SetActive(true);
            GameManager.Instance.gameState = GameState.Menu;
        }
    }
}
