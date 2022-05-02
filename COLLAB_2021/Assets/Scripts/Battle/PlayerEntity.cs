using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public GameObject inventory;
    public GameObject partyMenu;
    public GameObject pauseMenu;
    GameObject curMenu;
    [SerializeField] TopDownMovement movement; //TopDownMovement class reference
    public CharacterParty party;

    public void HandleMovement()
    {
        movement.Movements();
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ActivateMenu(pauseMenu);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ActivateMenu(inventory);
        }

        if (Input.GetKeyDown(KeyCode.U)) 
        {
            ActivateMenu(partyMenu);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {

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
