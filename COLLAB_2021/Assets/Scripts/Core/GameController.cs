using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Player References")]
    public PlayerEntity player;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] GameObject worldCamera;
    [SerializeField] DialogueManager dialogueManager; //dialogueManager Ref
    
    GameState state;

    public static GameController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        dialogueManager = FindObjectOfType<DialogueManager>(); //Auto populate the variable with dialogue manager class in the scene
        ////Deactivate cursor.
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        ConditionsDB.Init();
    }

    [Header("Battle References")]
    public EnemyEntity enemy;

    public void StartBattle(EnemyEntity enemy)
    {
        //Change game state and enable battle UI.
        state = GameState.Battle;
        //battleSystem.gameObject.SetActive(true);
        this.enemy = enemy;

        worldCamera.SetActive(false);

        var playerParty = GameManager.Instance.GetComponent<CharacterParty>();
        var enemyParty = enemy.GetComponent<CharacterParty>();

        GameManager.Instance.gameState = GameState.Battle;

        SceneManager.LoadScene("TestBattle", LoadSceneMode.Additive);

        //Start battle.
        //battleSystem.StartBattle(playerParty, enemyParty);
    }

    private void Update()
    {
        state = GameManager.Instance.gameState;

        //if (state == GameState.FreeRoam)
        //{
        //    if (!r_topDownMovement) return;
        //    //if player is not in dialogue then he/she can move
        //    if (!dialogueManager.inDialogue) r_topDownMovement.Movements();
        //}
        //else if (state == GameState.Battle)
        //{
        //    battleSystem.HandleUpdate();
        //}
        //else if(state == GameState.Menu)
        //{
        //    //TODO: menu.
        //}

        switch (state)
        {
            case GameState.FreeRoam:
                player.HandleInput();
                player.HandleMovement();
                break;
            case GameState.Battle:
                battleSystem.HandleUpdate();
                break;
            case GameState.Menu:
                player.HandleInput();
                break;
            case GameState.Dialogue:
                dialogueManager.HandleUpdate();
                break;
            case GameState.UpInventory:
                break;
            case GameState.Paused:
                break;
            default:
                break;
        }
    }
}
