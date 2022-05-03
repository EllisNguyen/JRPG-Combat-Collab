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

    [FoldoutGroup("Player")][SerializeField] BattleHud playerHud;
    [FoldoutGroup("Player")][SerializeField] List<BattlePawn> playerUnits;

    [PropertySpace(10)]

    [FoldoutGroup("Enemy")][SerializeField] BattleHud enemyHud;
    [FoldoutGroup("Enemy")][SerializeField] List<BattlePawn> enemyUnits;

    [PropertySpace(10)]

    //[SerializeField] BattleDialogueBox dialogueBox;
    [FoldoutGroup("Battle Components")][SerializeField] GameObject playerPawn_Pref;
    [FoldoutGroup("Battle Components")][SerializeField] GameObject enemyPawn_Pref;
    [FoldoutGroup("Battle Components")][SerializeField] InventoryUI inventoryUI;
    [FoldoutGroup("Battle Components")][SerializeField] BattleDialogue dialogueBox;

    [PropertySpace(10)]

    [FoldoutGroup("Entity")][ReadOnly] public CharacterParty playerParty;
    [FoldoutGroup("Entity")][ReadOnly] public CharacterParty enemyParty;

    [FoldoutGroup("Entity")][ReadOnly] public PlayerEntity player;
    [FoldoutGroup("Entity")][ReadOnly] public EnemyEntity enemy;

    //[BoxGroup("Party")] public CharacterParty playerParty;
    //[BoxGroup("Party")] public CharacterParty enemyParty;

    public event Action<bool> OnBattleOver;

    [PropertySpace(10)]

    [FoldoutGroup("Spawnpoints")]public List<Transform> playerSpawnPoints;
    [FoldoutGroup("Spawnpoints")] public List<Transform> enemySpawnPoints;

    [FoldoutGroup("Battle Multiplier")][SerializeField] float speedProgressorMultiplier = 0.03f;

    public float SpeedProgressorMultiplier => speedProgressorMultiplier;
    public BattleState State
    {
        get { return state; }
        set { state = value; }
    }

    [FoldoutGroup("Speed progressor")][SerializeField] RectTransform progressorHolder;
    [FoldoutGroup("Speed progressor")][SerializeField] Image activeCharacter;
    [FoldoutGroup("Speed progressor")][SerializeField] SpeedProgressor speedProgressorPrefab;
    [FoldoutGroup("Speed progressor")][SerializeField] List<SpeedProgressor> playerProgressors;
    [FoldoutGroup("Speed progressor")][SerializeField] List<SpeedProgressor> enemyProgressors;

    [SerializeField] BattlePawn activeUnit;
    [SerializeField] Image activeSprite;

    public BattlePawn ActiveUnit
    {
        get { return activeUnit; }
        set { activeUnit = value; }
    }

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

        StartCoroutine(SetupBattle());
        //WaitingForTurn();
    }

    /// <summary>
    /// Reset the speed progressor for everyone.
    /// </summary>
    void ResetSpeedProgressor()
    {
        activeCharacter.sprite = null;
        activeUnit = null;
        activeSprite.enabled = false;

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

    public void Start()
    {
        //SetupBattle();
    }

    public IEnumerator SetupBattle()
    {
        playerUnits[0].Setup(playerParty.GetHealthyCharacter());
        enemyUnits[0].Setup(enemyParty.GetHealthyCharacter());

        playerHud.SetData(playerUnits[0].Character);
        enemyHud.SetData(enemyUnits[0].Character);
        enemyHud.DisableNonPlayerElement();


        ResetSpeedProgressor();

        yield return dialogueBox.TypeDialogue($"You stumbled upon enemy {enemyUnits[0].Character.Base.charName}.");
        dialogueBox.SetSkillList(playerUnits[0].Character.Moves);

        yield return new WaitForSecondsRealtime(1f);

        state = BattleState.Waiting;
    }

    public void HandleUpdate()
    {
        switch (state)
        {
            case BattleState.Start:
                Debug.Log("bro");
                //WaitingForTurn();
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
                if(activeUnit != null)
                {
                    if (activeUnit == playerUnits[0])
                    {
                        StartCoroutine(PlayerAction());
                    }
                    else ResetEnemyProgressor();
                }
                break;
            case BattleState.Inventory:

                break;
            case BattleState.BattleOver:

                break;
            default:
                break;
        }
    }

    IEnumerator PlayerAction()
    {
        state = BattleState.ActionSelection;
        activeSprite.enabled = true;
        activeSprite.sprite = playerUnits[0].Character.Base.portraitSprite;
        yield return dialogueBox.TypeDialogue("Select your next move.");
        dialogueBox.EnableActionSelector(true);
    }

    void WaitingForTurn()
    {
        activeCharacter.sprite = null;
        activeUnit = null;
        activeSprite.enabled = false;

        for (int i = 0; i < playerUnits.Count; i++)
        {
            foreach (SpeedProgressor progressor in playerProgressors)
            {
                StartCoroutine(progressor.SpeedProgress(playerUnits[i].Character, playerUnits[0]));
            }
        }

        for (int j = 0; j < enemyUnits.Count; j++)
        {
            foreach (SpeedProgressor progressor in enemyProgressors)
            {
                StartCoroutine(progressor.SpeedProgress(enemyUnits[j].Character, enemyUnits[0]));
            }
        }
    }

    #region HANDLE ACTION SELECTION

    public void Attack()
    {

    }

    public void Guard()
    {

    }

    public void Skill()
    {
        dialogueBox.EnableSkillSelector(true);
    }

    public void Bag()
    {

    }

    public void Flee()
    {

    }

    #endregion HANDLE ACTION SELECTION

    [Button]
    public void ResetEnemyProgressor()
    {
        if (state != BattleState.Waiting) state = BattleState.Waiting;
        activeUnit = null;
        enemyProgressors[0].Slider.value = 0;
    }

    [Button]
    public void ResetPlayerProgressor()
    {
        if(state != BattleState.Waiting) state = BattleState.Waiting;
        activeUnit = null;
        playerProgressors[0].Slider.value = 0;
    }
}
