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

    [FoldoutGroup("Requires entities")][ReadOnly] public CharacterParty playerParty;
    [FoldoutGroup("Requires entities")][ReadOnly] public CharacterParty enemyParty;

    [FoldoutGroup("Requires entities")][ReadOnly] public PlayerEntity player;
    [FoldoutGroup("Requires entities")][ReadOnly] public EnemyEntity enemy;

    //[BoxGroup("Party")] public CharacterParty playerParty;
    //[BoxGroup("Party")] public CharacterParty enemyParty;

    public event Action<bool> OnBattleOver;

    [PropertySpace(10)]

    [FoldoutGroup("Spawnpoints")]public List<Transform> playerSpawnPoints;
    [FoldoutGroup("Spawnpoints")] public List<Transform> enemySpawnPoints;

    [FoldoutGroup("Battle Multiplier")][SerializeField] float speedProgressorMultiplier = 0.03f;

    [FoldoutGroup("Basic Move")] public MoveData basicAttack;
    [FoldoutGroup("Basic Move")] public MoveData basicGuard;

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

    [FoldoutGroup("Active entity")][SerializeField] BattlePawn activeUnit;
    [FoldoutGroup("Active entity")][SerializeField] Image activeSprite;

    int escapeAttempt;
    MoveData moveToLearn;

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
        GameController.Instance.SubToBattleEnd();
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

    IEnumerator HandleCreatureFainted(BattlePawn faintedUnit)
    {
        //Display enemy fainted dialogue and play faint animation.
        yield return dialogueBox.TypeDialogue($"{faintedUnit.Character.Base.charName.ToUpper()} fainted.");

        //Enemy fainted and player won.
        yield return new WaitForSeconds(2f);

        //Calculate EXP gained if enemy fainted.
        if (!faintedUnit.IsPlayerUnit)
        {
            //EXP gains.
            var expYield = faintedUnit.Character.Base.expYield;
            int enemyLevel = faintedUnit.Character.Level;

            int expGain = Mathf.FloorToInt((expYield * enemyLevel) / 7);
            playerUnits[0].Character.Exp += expGain;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            yield return dialogueBox.TypeDialogue($"{playerUnits[0].Character.Base.charName.ToUpper()} gained {expGain} exp.");
            yield return playerHud.SetExpSmooth();

            //Check level up.
            //Update the HUD.
            //Learn new move(s).
            while (playerUnits[0].Character.CheckForLevelUp())
            {
                playerHud.SetLevel();
                yield return dialogueBox.TypeDialogue($"{playerUnits[0].Character.Base.charName.ToUpper()} grew to level {playerUnits[0].Character.Level}.");

                yield return playerHud.SetExpSmooth(true);
            }

            yield return new WaitForSeconds(1f);
        }

        CheckForBattleOver(faintedUnit);
    }

    //Check for the Unit that is fainted is Player's or Opponent's.
    void CheckForBattleOver(BattlePawn faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            //Declare a check for healthy creature in party.
            var nextCreature = playerParty.GetHealthyCharacter();

            BattleOver(false);
        }
        else
        {
            BattleOver(true);

        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        //Update crit dialogue when creature landed a crit.
        if (damageDetails.Critical > 1)
            yield return dialogueBox.TypeDialogue("Holy shit it's a crit.");

        //Update effectiveness dialogue.
        if (damageDetails.TypeEffectiveness > 1)
            yield return dialogueBox.TypeDialogue("Damn boi, that's a lot of damage.");
        else if (damageDetails.TypeEffectiveness < 1)
            yield return dialogueBox.TypeDialogue("What a weak ass skill, try another.");

    }

    /// <summary>
    /// Trigger the battle over state.
    /// </summary>
    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;

        //Loop through all character in party.
        //Reset all boosted stats.
        playerParty.Characters.ForEach(c => c.OnBattleOver());

        //Notify the game that the battle is over.
        OnBattleOver(won);

        enemy.gameObject.SetActive(false);
    }

    #region HANDLE ACTION SELECTION

    public void Attack()
    {
        //TODO: init basic attack.
    }

    public void Guard()
    {
        //TODO: init basic guard.
    }

    public void Skill()
    {
        dialogueBox.EnableSkillSelector(true);
    }

    public void Bag()
    {
        //TODO: open inventory.
    }

    public void Flee()
    {
        dialogueBox.EnableActionSelector(false);
        StartCoroutine(TryToEscape());
    }

    #endregion HANDLE ACTION SELECTION

    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;

        ++escapeAttempt;

        //Get player's and enemy's speed.
        int playerSpeed = playerUnits[0].Character.Base.speed;
        int enemySpeed = enemyUnits[0].Character.Base.speed;

        if (enemySpeed < playerSpeed)
        {
            yield return dialogueBox.TypeDialogue("YESS WE OUT RUN THAT BITCH.");
            BattleOver(true);
        }
        else
        {
            //Battle escape calculation.
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeAttempt;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                yield return dialogueBox.TypeDialogue("YESS WE OUT RUN THAT BITCH.");
                BattleOver(true);
            }
            //Continue battle when escape failed.
            else
            {
                yield return dialogueBox.TypeDialogue("Damn that thing's still on our tails.");
                state = BattleState.RunningTurn;
            }
        }
    }

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
