using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] TopDownMovement r_topDownMovement; //TopDownMovement class reference
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    public static GameController Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        ////Deactivate cursor.
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        ConditionsDB.Init();
    }

    EnemyEntity enemy;

    public void StartBattle(EnemyEntity enemy)
    {
        //Change game state and enable battle UI.
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        this.enemy = enemy;

        worldCamera.gameObject.SetActive(false);

        var playerParty = GameManager.Instance.GetComponent<CharacterParty>();
        var enemyParty = enemy.GetComponent<CharacterParty>();

        //Start battle.
        battleSystem.StartBattle(playerParty, enemyParty);
    }

    private void Update()
    {
        state = GameManager.Instance.gameState;

        if (state == GameState.FreeRoam)
        {
            if (!r_topDownMovement) return;
            r_topDownMovement.Movements();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if(state == GameState.Menu)
        {
            //TODO: menu.
        }
    }
}
