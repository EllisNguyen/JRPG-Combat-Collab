///Author: Phap Nguyen.
///Description: Game controller that hold the information of current play session.
///Day created: 22/03/2022
///Last edited: 03/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GameController : MonoBehaviour
{
    [Header("Player References")]
    public PlayerEntity player;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] DialogueManager dialogueManager; //dialogueManager Ref

    [SerializeField] LayerMask freeroamLayer;
    [SerializeField] LayerMask battleLayer;
    
    GameState state;

    [FoldoutGroup("Element color")]public Color normal;
    [FoldoutGroup("Element color")]public Color heat;
    [FoldoutGroup("Element color")]public Color electric;
    [FoldoutGroup("Element color")]public Color radiation;
    [FoldoutGroup("Element color")]public Color ice;
    [FoldoutGroup("Element color")]public Color light;
    [FoldoutGroup("Element color")]public Color dark;

    [FoldoutGroup("Element icon")]public Sprite normalIcon;
    [FoldoutGroup("Element icon")]public Sprite heatIcon;
    [FoldoutGroup("Element icon")]public Sprite electricIcon;
    [FoldoutGroup("Element icon")]public Sprite radiationIcon;
    [FoldoutGroup("Element icon")]public Sprite iceIcon;
    [FoldoutGroup("Element icon")]public Sprite lightIcon;
    [FoldoutGroup("Element icon")]public Sprite darkIcon;

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

        CharacterDB.Init();
        MoveDB.Init();
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

        //worldCamera.SetActive(false);
        worldCamera.cullingMask = battleLayer;

        var playerParty = GameManager.Instance.GetComponent<CharacterParty>();
        var enemyParty = enemy.GetComponent<CharacterParty>();

        GameManager.Instance.gameState = GameState.Battle;

        SceneManager.LoadScene("TestBattle", LoadSceneMode.Additive);

        enemy.gameObject.SetActive(false);
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

    public void SubToBattleEnd()
    {
        battleSystem.OnBattleOver += EndBattle;
    }

    public void EndBattle(bool won)
    {
        StartCoroutine(BattleEndSequence());
    }

    public IEnumerator BattleEndSequence()
    {
        GameManager.Instance.FadeOut();

        yield return new WaitUntil(() => GameManager.Instance.isFading == false);

        SceneManager.UnloadSceneAsync("TestBattle");
        battleSystem = null;
        worldCamera.cullingMask = freeroamLayer;

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.gameState = GameState.FreeRoam;
        //worldCamera.SetActive(true);

        //GameManager.Instance.FadeIn();

        GameManager.Instance.FadeIn();
    }

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

    public void SetCurrentScene(SceneDetails curScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = curScene;
    }

    public void SaveSlot(int num)
    {
        SavingSystem.i.Save($"SaveSlot{num}");
    }

    public void LoadSlot(int num)
    {
        SavingSystem.i.Load($"SaveSlot{num}");
        player.Movement.ResetNavMesh();
    }
}
