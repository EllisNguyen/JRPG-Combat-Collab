///Author: Phap Nguyen.
///Description: Game controller that hold the information of current play session.
///Day created: 22/03/2022
///Last edited: 03/05/2022 - Phap Nguyen.

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

    public BattleSystem BattleSystem
    {
        get { return battleSystem; }
        set { battleSystem = value; }
    }

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
