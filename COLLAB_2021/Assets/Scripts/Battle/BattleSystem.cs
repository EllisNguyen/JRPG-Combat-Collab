using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, ActionSelection, MoveSelection, RunningTurn, Busy, Inventory, BattleOver }

public class BattleSystem : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] List<BattlePawn> playerUnits;

    [Header("Enemy")]
    [SerializeField] List<BattlePawn> enemyUnits;

    [Header("Battle component")]
    //[SerializeField] BattleDialogueBox dialogueBox;
    [SerializeField] GameObject playerPawn_Pref;
    [SerializeField] GameObject enemyPawn_Pref;
    [SerializeField] InventoryUI inventoryUI;

    CharacterParty playerParty;
    CharacterParty enemyParty;

    PlayerEntity player;
    EnemyEntity enemy;

    public event Action<bool> OnBattleOver;
    BattleState state;

    [Header("SpawnPoints")]
    public List<Transform> playerSpawnPoints;
    public List<Transform> enemySpawnPoints;

    public void StartBattle(CharacterParty playerParty, CharacterParty enemyParty)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;

        //player = playerParty.GetComponent<PlayerEntity>();
        //enemy = enemyParty.GetComponent<EnemyEntity>();

        SetupBattle();
    }

    int playerRandomSpawn;
    int enemyRandomSpawn;

    public void SetupBattle()
    {
        
        

        print(playerRandomSpawn);

        for (int i = 0; i < playerParty.Characters.Count; i++)
        {
            playerRandomSpawn = UnityEngine.Random.Range(0, playerSpawnPoints.Count);
            print("step 1: PLAYER party include " + playerParty.Characters.Count + " characters.");
            var player = Instantiate(playerPawn_Pref, playerSpawnPoints[playerRandomSpawn].transform.position, Quaternion.identity);
            playerUnits.Add(player.GetComponent<BattlePawn>());

            if (playerUnits.Count >= 2) break;
        }

        ////Loop through player party and setup the spawn.
        //for (int j = 0; j < playerUnits.Count; j++)
        //{
        //    print("number of player units are = " + playerUnits.Count);
        //    playerUnits[j].Setup(playerParty.GetHealthyCharacter());
        //}

        for (int k = 0; k < enemyParty.Characters.Count; k++)
        {
            enemyRandomSpawn = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
            print("step 1: ENEMY party include " + enemyParty.Characters.Count + " monster.");
            var enemy = Instantiate(enemyPawn_Pref, enemySpawnPoints[enemyRandomSpawn].transform.position, Quaternion.identity);
            enemyUnits.Add(enemy.GetComponent<BattlePawn>());
        }

        print("hello");
        ////Loop through enemy party and setup the spawn.
        //for (int l = 0; l < enemyUnits.Count; l++)
        //{
        //    enemyUnits[l].Setup(enemyParty.GetHealthyCharacter());
        //}
    }

    public void HandleUpdate()
    {
        //ACTION SELECTION.
        if (state == BattleState.ActionSelection)
        {
            
        }
        //MOVE SELECTION.
        else if (state == BattleState.MoveSelection)
        {
            
        }
    }
}
