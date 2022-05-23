///Author: Phap Nguyen.
///Description: Hold all player components.
///Day created: 01/05/2022
///Last edited: 17/05/2022 - Phap Nguyen.

using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerEntity : MonoBehaviour
{
    [Header("Menu")]
    public GameObject inventory;
    public GameObject partyMenu;
    public GameObject pauseMenu;
    GameObject curMenu;
    [SerializeField] TopDownMovement movement; //TopDownMovement class reference
    public CharacterParty party;

    [Header("Values")]
    [SerializeField] int money;
    [SerializeField] Vector3 position;
    [SerializeField] ParticleSystem increaseMana;
    [SerializeField] float healTimer = 5f;
    [SerializeField] float curTimer;

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    private void Start()
    {
        curTimer = healTimer;
    }

    public void HandleMovement()
    {
        movement.Movements();
        position = this.gameObject.transform.position;

        for (int i = 0; i < party.Characters.Count; i++)
        {
            if (party.Characters[i].MP < party.Characters[i].MaxMP)
            {
                healTimer -= Time.deltaTime;
                if (healTimer < 0)
                {
                    increaseMana.Play();
                    party.Characters[i].IncreaseMP(3);
                    healTimer = curTimer;
                }
            }
        }
    }

    [Button]
    void minusMana()
    {
        foreach (var character in party.Characters)
        {
            character.DecreaseMP(10);
        }
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
