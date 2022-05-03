///Author: Phap Nguyen.
///Description: Battle system for the game.
///Day created: 22/12/2022
///Last edited: 03/05/2022 - Phap Nguyen.

using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum BattleState { Start, Waiting, ActionSelection, MoveSelection, RunningTurn, Busy, Inventory, BattleOver }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleState state;

    [Header("Player")]
    [SerializeField] BattleHud playerHud;
    [SerializeField] List<BattlePawn> playerUnits;

    [Header("Enemy")]
    [SerializeField] BattleHud enemyHud;
    [SerializeField] List<BattlePawn> enemyUnits;

    [Header("Battle component")]
    //[SerializeField] BattleDialogueBox dialogueBox;
    [SerializeField] GameObject playerPawn_Pref;
    [SerializeField] GameObject enemyPawn_Pref;
    [SerializeField] InventoryUI inventoryUI;

    [BoxGroup("Entity")][ReadOnly] public CharacterParty playerParty;
    [BoxGroup("Entity")][ReadOnly] public CharacterParty enemyParty;

    [BoxGroup("Entity")][ReadOnly] public PlayerEntity player;
    [BoxGroup("Entity")][ReadOnly] public EnemyEntity enemy;

    //[BoxGroup("Party")] public CharacterParty playerParty;
    //[BoxGroup("Party")] public CharacterParty enemyParty;

    public event Action<bool> OnBattleOver;

    [Header("SpawnPoints")]
    public List<Transform> playerSpawnPoints;
    public List<Transform> enemySpawnPoints;

    [BoxGroup("Battle Multiplier")][SerializeField] float speedProgressorMultiplier = 0.3f;

    public float SpeedProgressorMultiplier => speedProgressorMultiplier;
    public BattleState State
    {
        get { return state; }
        set { state = value; }
    }

    [BoxGroup("Speed progressor")][SerializeField] RectTransform progressorHolder;
    [BoxGroup("Speed progressor")][SerializeField] Image activeCharacter;
    [BoxGroup("Speed progressor")][SerializeField] SpeedProgressor speedProgressorPrefab;
    [BoxGroup("Speed progressor")][SerializeField] List<SpeedProgressor> playerProgressors;
    [BoxGroup("Speed progressor")][SerializeField] List<SpeedProgressor> enemyProgressors;

    public Image ActiveCharacter
    {
        get { return activeCharacter; }
        set { activeCharacter = value; }
    }

    void OnEnable()
    {
        GameController.Instance.BattleSystem = this;
    }

    public void StartBattle(CharacterParty playerParty, CharacterParty enemyParty)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;

        //player = playerParty.GetComponent<PlayerEntity>();
        //enemy = enemyParty.GetComponent<EnemyEntity>();

        SetupBattle();
        //WaitingForTurn();
    }

    /// <summary>
    /// Reset the speed progressor for everyone.
    /// </summary>
    void ResetSpeedProgressor()
    {
        foreach (Transform child in progressorHolder.transform)
        {
            //Destroy them all.
            Destroy(child.gameObject);
        }

        //Create a new list after destroy all the children.
        playerProgressors = new List<SpeedProgressor>();
        enemyProgressors = new List<SpeedProgressor>();

        //Add player progressor onto correct position.
        foreach (BattlePawn playerUnit in playerUnits)
        {
            var progresorObj = Instantiate(speedProgressorPrefab, progressorHolder.transform);
            progresorObj.SetProgressorData(playerUnits[0].Character);
            playerProgressors.Add(progresorObj);
        }

        //Add enemy progressor onto correct position.
        foreach (BattlePawn enemyUnit in enemyUnits)
        {
            var progresorObj = Instantiate(speedProgressorPrefab, progressorHolder.transform);
            progresorObj.SetProgressorData(enemyUnits[0].Character);
            enemyProgressors.Add(progresorObj);
        }
    }

    int playerRandomSpawn;
    int enemyRandomSpawn;

    public void Start()
    {
        //SetupBattle();
    }

    public void SetupBattle()
    {
        //foreach (var player in playerUnits)
        //{
        //    playerUnits[0].Setup();
        //}

        //foreach (var enemy in enemyUnits)
        //{
        //    enemy.Setup();
        //}

        playerUnits[0].Setup(playerParty.GetHealthyCharacter());
        enemyUnits[0].Setup(enemyParty.GetHealthyCharacter());

        playerHud.SetData(playerUnits[0].Character);
        enemyHud.SetData(enemyUnits[0].Character);
        enemyHud.DisableNonPlayerElement();

        ResetSpeedProgressor();

        //print(playerRandomSpawn);

        //for (int i = 0; i < playerParty.Characters.Count; i++)
        //{
        //    playerRandomSpawn = UnityEngine.Random.Range(0, playerSpawnPoints.Count);
        //    print("step 1: PLAYER party include " + playerParty.Characters.Count + " characters.");
        //    var player = Instantiate(playerPawn_Pref, playerSpawnPoints[playerRandomSpawn].transform.position, Quaternion.identity);
        //    playerUnits.Add(player.GetComponent<BattlePawn>());

        //    if (playerUnits.Count >= 2) break;
        //}

        //////Loop through player party and setup the spawn.
        ////for (int j = 0; j < playerUnits.Count; j++)
        ////{
        ////    print("number of player units are = " + playerUnits.Count);
        ////    playerUnits[j].Setup(playerParty.GetHealthyCharacter());
        ////}

        //for (int k = 0; k < enemyParty.Characters.Count; k++)
        //{
        //    enemyRandomSpawn = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
        //    print("step 1: ENEMY party include " + enemyParty.Characters.Count + " monster.");
        //    var enemy = Instantiate(enemyPawn_Pref, enemySpawnPoints[enemyRandomSpawn].transform.position, Quaternion.identity);
        //    enemyUnits.Add(enemy.GetComponent<BattlePawn>());
        //}

        //print("hello");
        //////Loop through enemy party and setup the spawn.
        ////for (int l = 0; l < enemyUnits.Count; l++)
        ////{
        ////    enemyUnits[l].Setup(enemyParty.GetHealthyCharacter());
        ////}
    }

    public void HandleUpdate()
    {
        switch (state)
        {
            case BattleState.Start:
                Debug.Log("bro");
                WaitingForTurn();
                break;
            case BattleState.Waiting:
                Debug.Log("waiting to choose move");
                WaitingForTurn();
                break;
            case BattleState.ActionSelection:

                break;
            case BattleState.MoveSelection:

                break;
            case BattleState.RunningTurn:

                break;
            case BattleState.Busy:

                break;
            case BattleState.Inventory:

                break;
            case BattleState.BattleOver:

                break;
            default:
                break;
        }
    }

    void WaitingForTurn()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            foreach (SpeedProgressor progressor in playerProgressors)
            {
                StartCoroutine(progressor.SpeedProgress(playerUnits[i].Character));
            }
        }

        for (int j = 0; j < enemyUnits.Count; j++)
        {
            foreach (SpeedProgressor progressor in enemyProgressors)
            {
                StartCoroutine(progressor.SpeedProgress(enemyUnits[j].Character));
            }
        }
    }

    [Button]
    public void ResetEnemyProgressor()
    {
        if (state != BattleState.Waiting) state = BattleState.Waiting;
        enemyProgressors[0].Slider.value = 0;
    }

    [Button]
    public void ResetPlayerProgressor()
    {
        if(state != BattleState.Waiting) state = BattleState.Waiting;
        playerProgressors[0].Slider.value = 0;
    }

    public void Attack()
    {
        state = BattleState.ActionSelection;
    }
    public void Guard()
    {
        state = BattleState.MoveSelection;
    }
}
