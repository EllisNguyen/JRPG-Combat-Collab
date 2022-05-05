///Author: Phap Nguyen.
///Description: Battle system for the game.
///Day created: 22/12/2022
///Last edited: 05/05/2022 - Phap Nguyen.

using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum BattleState { Start, Waiting, ActionSelection, MoveSelection, RunningTurn, Busy, Inventory, BattleOver }

public enum BattleAction { Move, SwitchCharacter, UseItem, Run, Wait }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleState state;

    [FoldoutGroup("Player")] [SerializeField] BattleHud playerHud;
    [FoldoutGroup("Player")] [SerializeField] List<BattlePawn> playerUnits;

    [PropertySpace(10)]

    [FoldoutGroup("Enemy")] [SerializeField] BattleHud enemyHud;
    [FoldoutGroup("Enemy")] [SerializeField] List<BattlePawn> enemyUnits;

    [PropertySpace(10)]

    [FoldoutGroup("Battle Components")] [SerializeField] GameObject playerPawn_Pref;
    [FoldoutGroup("Battle Components")] [SerializeField] GameObject enemyPawn_Pref;
    [FoldoutGroup("Battle Components")] [SerializeField] InventoryUI inventoryUI;
    [FoldoutGroup("Battle Components")] [SerializeField] BattleDialogue dialogueBox;

    public BattleDialogue DialogueBox => dialogueBox;

    [PropertySpace(10)]

    [FoldoutGroup("Requires entities")] [ReadOnly] public CharacterParty playerParty;
    [FoldoutGroup("Requires entities")] [ReadOnly] public CharacterParty enemyParty;

    [FoldoutGroup("Requires entities")] [ReadOnly] public PlayerEntity player;
    [FoldoutGroup("Requires entities")] [ReadOnly] public EnemyEntity enemy;

    //[BoxGroup("Party")] public CharacterParty playerParty;
    //[BoxGroup("Party")] public CharacterParty enemyParty;

    public event Action<bool> OnBattleOver;

    [PropertySpace(10)]

    [FoldoutGroup("Spawnpoints")] public List<Transform> playerSpawnPoints;
    [FoldoutGroup("Spawnpoints")] public List<Transform> enemySpawnPoints;

    [FoldoutGroup("Battle Multiplier")] [SerializeField] float speedProgressorMultiplier = 0.03f;

    [FoldoutGroup("Basic Move")] public MoveData basicAttack;
    [FoldoutGroup("Basic Move")] public MoveData basicGuard;
    [FoldoutGroup("Basic Move")] public SkillPanel currentMovePanel;
    [FoldoutGroup("Basic Move")] public Move currentMove;

    public float SpeedProgressorMultiplier => speedProgressorMultiplier;
    public BattleState State
    {
        get { return state; }
        set { state = value; }
    }

    [FoldoutGroup("Speed progressor")] [SerializeField] RectTransform progressorHolder;
    [FoldoutGroup("Speed progressor")] [SerializeField] Image activeCharacter;
    [FoldoutGroup("Speed progressor")] [SerializeField] SpeedProgressor speedProgressorPrefab;
    [FoldoutGroup("Speed progressor")] [SerializeField] List<SpeedProgressor> playerProgressors;
    [FoldoutGroup("Speed progressor")] [SerializeField] List<SpeedProgressor> enemyProgressors;

    [FoldoutGroup("Active entity")] [SerializeField] BattlePawn activeUnit;
    [FoldoutGroup("Active entity")] [SerializeField] Image activeSprite;

    int escapeAttempt;
    MoveData moveToLearn;
    public BattleAction action;

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

        playerUnits[0].Hud = playerHud;
        enemyUnits[0].Hud = enemyHud;

        playerHud.SetData(playerUnits[0].Character);
        enemyHud.SetData(enemyUnits[0].Character);

        enemyHud.DisableNonPlayerElement();

        ResetSpeedProgressor();

        yield return dialogueBox.TypeDialogue($"You stumbled upon enemy {enemyUnits[0].Character.Base.charName}.");
        dialogueBox.SetSkillList(playerUnits[0].Character.Moves);

        //yield return new WaitForSecondsRealtime(1f);

        //yield return dialogueBox.TypeDialogue($"Ready for action.");
        state = BattleState.Waiting;
    }

    public void BasicAttack()
    {
        var move = new Move(basicAttack);

        //UseSKill(playerUnits[0], enemyUnits[0], move);
        //RunMove()
        StartCoroutine(RunTurnsPlayer(BattleAction.Move));
    }

    public void BasicGuard()
    {
        var move = new Move(basicGuard);

        StartCoroutine(RunTurnsPlayer(BattleAction.Move));
    }

    public void UseSKill(BattlePawn caster, BattlePawn receiver, Move move)
    {
        state = BattleState.RunningTurn;
    }

    public void HandleUpdate()
    {
        switch (state)
        {
            case BattleState.Start:
                //WaitingForTurn();
                break;
            case BattleState.Waiting:
                Debug.Log("waiting to choose move");
                WaitingForTurn();
                break;
            case BattleState.ActionSelection:
                if (currentMove != null) print(currentMove.Base.Name);
                break;
            case BattleState.MoveSelection:

                break;
            case BattleState.RunningTurn:
                
                break;
            case BattleState.Busy:
                ActionSelection();
                break;
            case BattleState.Inventory:

                break;
            case BattleState.BattleOver:

                break;
            default:
                break;
        }
    }

    void HandleMoveSelection()
    {
        //Can only move between 0 and current number of moves.
        //currentMove = ;

        //dialogueBox.UpdateMoveSelection(currentMove, playerUnits[0].Character.CurrentMove);

        //SELECT A MOVE
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            var move = playerUnits[0].Character.CurrentMove;
            if (playerUnits[0].Character.Base.mana == 0) return;

            //dialogueBox.EnableMoveSelector(false);
            dialogueBox.EnableDialogueText(true);
            StartCoroutine(RunTurnsPlayer(BattleAction.Move));
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
        {
            //dialogueBox.EnableMoveSelector(false);
            dialogueBox.EnableDialogueText(true);
            //ActionSelection();
        }
    }

    void PlayerAction()
    {
        state = BattleState.ActionSelection;
        activeSprite.enabled = true;
        activeSprite.sprite = playerUnits[0].Character.Base.portraitSprite;

        dialogueBox.SetDialogue("Select your next move.");

        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableActionSelector(true);
    }

    void WaitingForTurn()
    {
        activeCharacter.sprite = null;
        activeUnit = null;
        activeSprite.enabled = false;

        dialogueBox.EnableDialogueText(false);

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

    IEnumerator HandleCharacterFainted(BattlePawn faintedUnit)
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

    //
    public IEnumerator RunTurnsPlayer(BattleAction playerAction)
    {
        //Perform player's turn.
        state = BattleState.RunningTurn;

        //Check if player perform a move.
        if (playerAction == BattleAction.Move)
        {
            //PLAYER.
            //Store the current move into player's current creature data.
            //playerUnits[0].Character.CurrentMove = currentMove;

            //OPPONENT.
            //Store the current move into player's current creature data (as randomly choose one).

            //enemyUnits[0].Character.CurrentMove = enemyUnits[0].Character.GetRandomMove();

            //Move FIRST TURN and SECOND TURN base on the creature's SPEED stat.
            //First turn.

            yield return RunMove(activeUnit, enemyUnits[0], currentMove);
            yield return RunAfterTurn(activeUnit);//End turn.
            //ResetPlayerProgressor();

            if (state == BattleState.BattleOver) yield break;
            ////Exec if second creature still live.
            //if (secondCharacter.HP > 0)
            //{
            //    //Second turn.
            //    yield return RunMove(secondUnit, firstUnit, secondUnit.Character.CurrentMove);
            //    yield return RunAfterTurn(secondUnit);//End turn.
            //    if (state == BattleState.BattleOver) yield break;
            //}
        }

        ////Check if player try to switch creature.
        //if (playerAction == BattleAction.SwitchCharacter)
        //{
        //    //var selectedCharacter = partyScreen.SelectedMember;
        //    state = BattleState.Busy;
        //    //yield return SwitchCharacter(selectedCharacter);
        //}
        ////Check if player try to use items.
        //else if (playerAction == BattleAction.UseItem)
        //{
        //    //This handle from item screen, do nothing and skip the turn after used the item.
        //    dialogueBox.EnableActionSelector(false);
        //}
        ////Escape wild battle.
        else if (playerAction == BattleAction.Run)
        {
            yield return TryToEscape();
        }

        //Return to action selection if battle is not over.
        if (state != BattleState.BattleOver)
        {
            //ActionSelection();
            state = BattleState.Busy;
        }
    }

    public IEnumerator RunTurnsEnemy(BattleAction playerAction)
    {
        //Perform player's turn.
        state = BattleState.RunningTurn;

        //Enemy turn.
        var enemyMove = activeUnit.Character.GetRandomMove();
        yield return RunMove(activeUnit, playerUnits[0], enemyMove);
        yield return RunAfterTurn(activeUnit);//End turn.
        if (state == BattleState.BattleOver) yield break;
    }

    public void ActionSelection()
    {
        dialogueBox.EnableDialogueText(true);
        if (activeUnit != null)
        {
            if (activeUnit == playerUnits[0])
            {
                PlayerAction();
            }
            else ResetEnemyProgressor();
        }
    }

    #region Performing move logic
    //Object oriented scripting toward performing the move of the creature.
    //
    IEnumerator RunMove(BattlePawn sourceUnit, BattlePawn targetUnit, Move move)
    {
        //Declare a bool from the creature current status and check if any status will stop the creature move.
        bool canRunMove = sourceUnit.Character.OnBeforeMove();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Character);

            //Call the update health bar function.
            yield return sourceUnit.Hud.WaitForHpUpdate();

            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Character);

        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableDialogueText(true);

        //Decrease the PP of the move and fire the dialogue coroutine.
        //move.Mana--;
        print($"Used {move.Base.Name.ToUpper()}.");
        dialogueBox.SetDialogue($"{sourceUnit.Character.Base.charName.ToUpper()} used {move.Base.Name.ToUpper()}.");

        //Check if the attack landed.
        if (CheckIfMoveHits(move, sourceUnit.Character, targetUnit.Character))
        {
            //Perform a simple attack animation.
            sourceUnit.PlayAttackAnimation();
            //yield return new WaitForSeconds(0.5f);

            //Hit animation.
            targetUnit.PlayHitAnimation();

            //Check if the move is Status effect.
            if (move.Base.Category == MoveCategory.Status)
            {
                yield return RunMoveEffect(move.Base.Effect, sourceUnit.Character, targetUnit.Character, move.Base.Target);
            }
            //Do damage if not Status effect move.
            else
            {
                //Declare fainted boolean
                var damageDetails = targetUnit.Character.TakeDamage(move, sourceUnit.Character);

                //Call the update health bar func.
                yield return targetUnit.Hud.WaitForHpUpdate();

                //Show crit or type effectiveness dialogue.
                yield return ShowDamageDetails(damageDetails);
            }

            //Check for available secondary effects.
            //Check if secondary effect available.
            //Check for opponent health.
            if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnit.Character.HP > 0)
            {
                //Loop through all secondary effects.
                foreach (var secondary in move.Base.Secondaries)
                {
                    //Chance of secondary fx to occur.
                    var rnd = UnityEngine.Random.Range(1, 101);

                    //Compare random number to fixed chance.
                    if (rnd <= secondary.Chance)
                    {
                        yield return RunMoveEffect(secondary, sourceUnit.Character, targetUnit.Character, secondary.Target);
                    }
                }
            }

            //Check for the creature's health.
            if (targetUnit.Character.HP <= 0)
            {
                yield return HandleCharacterFainted(targetUnit);
            }
        }
        //If the move does not landed / missed.
        else
        {
            yield return dialogueBox.TypeDialogue($"{sourceUnit.Character.Base.charName.ToUpper()}'s attack missed.");
        }
    }

    //E
    IEnumerator RunMoveEffect(MoveEffect effects, Character source, Character target, MoveTarget moveTarget)
    {
        //STAT BOOSTING EFFECT.
        //Check if the move can actually do something.
        if (effects.Boost != null)
        {
            //Apply boost to source unit if target is SELF.
            if (moveTarget == MoveTarget.Self)
                source.ApplyBoost(effects.Boost);
            //Apply boost to source unit if target is FOE.
            else
                target.ApplyBoost(effects.Boost);
        }

        //STATUS CONDITION.
        //Check if the status move inflict any status condition.
        if (effects.Status != ConditionID.none)
        {
            //Apply the status condition.
            target.SetStatus(effects.Status);
        }

        //VOLATILE STATUS.
        //Check if the status move inflict any status condition.
        if (effects.VolatileStatus != ConditionID.none)
        {
            //Apply the status condition.
            target.SetVolatileStatus(effects.VolatileStatus);
        }

        //Display the status change messages.
        //Only display one that on the queue.
        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    //Exec when a turn end.
    IEnumerator RunAfterTurn(BattlePawn sourceUnit)
    {
        //Don't run the coroutin if battle is over.
        if (state == BattleState.BattleOver) yield break;

        //Continue until the state is RunningTurn.
        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        //Fire the OnAfterTurn() action.
        sourceUnit.Character.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Character);

        //Call the update health bar func.
        yield return sourceUnit.Hud.WaitForHpUpdate();

        //Check for the creature's health again if the status condition cause the creature to faint.
        if (sourceUnit.Character.HP <= 0)
        {
            yield return HandleCharacterFainted(sourceUnit);
            yield return new WaitUntil(() => state == BattleState.RunningTurn);
        }

        ResetPlayerProgressor();
    }

    //Boolean to calculate accuracy of the move, check if the move hit or not.
    //EVASION and ACCURACY.
    bool CheckIfMoveHits(Move move, Character source, Character target)
    {
        //Return true if the move never miss.
        if (move.Base.AlwaysHit)
            return true;

        //Declare the move accuracy.
        float moveAccuracy = move.Base.Accuracy;

        //Store the move acuracy in the battle.
        int accuracy = source.StatBoosts[Stat.accuracy];
        int evasion = source.StatBoosts[Stat.evasion];

        //An array of value that boosting the stat.
        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        //Buff or debuff accuracy if accuracy point is > or < 0.
        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        //Buff or debuff evasion if accuracy point is > or < 0.
        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        //Check if random generated number less than or equal to the move accuracy.
        return UnityEngine.Random.Range(1, 101) <= move.Base.Accuracy;
    }

    #endregion

    //
    IEnumerator SwitchCharacter(Character newCreature, bool isTrainerAboutToUse = false)
    {
        //Check for current creature HP.
        if (playerUnits[0].Character.HP > 0)
        {
            //Sequence of calling back the creature.
            yield return dialogueBox.TypeDialogue($"Come back {playerUnits[0].Character.Base.charName.ToUpper()}.");
            playerUnits[0].PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        //Send out the next healthy creature.
        playerUnits[0].Setup(newCreature);

        //
        //dialogueBox.SetMoveName(newCreature.Moves);

        //Set dialogue text with typing effect
        yield return (dialogueBox.TypeDialogue($"Do your best {newCreature.Base.charName.ToUpper()}!"));

        //if (isTrainerAboutToUse)
        //    StartCoroutine(SendNextTrainerCreature());
        //else
        //    state = BattleState.RunningTurn;
    }

    //
    IEnumerator ShowStatusChanges(Character character)
    {
        //Check if any messages stat change queue.
        while (character.StatusChanges.Count > 0)
        {
            //Dequeue the first message and store it in message var.
            var message = character.StatusChanges.Dequeue();

            //Display in dialogue box.
            yield return dialogueBox.TypeDialogue(message);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        //Update crit dialogue when creature landed a crit.
        if (damageDetails.Critical > 1)
            yield return dialogueBox.TypeDialogue("Holy shit it's a crit.");

        //Update effectiveness dialogue.
        if (damageDetails.ElementalModifier > 1)
            yield return dialogueBox.TypeDialogue("Damn boi, that's a lot of damage.");
        else if (damageDetails.ElementalModifier < 1)
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
        enemyParty.Characters.ForEach(e => e.OnBattleOver());

        //Notify the game that the battle is over.
        OnBattleOver(won);

        enemy.gameObject.SetActive(false);
    }

    #region HANDLE ACTION SELECTION

    public void Attack()
    {
        BasicAttack();
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
        StartCoroutine(RunTurnsPlayer(BattleAction.Run));
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
            playerUnits[0].PlayFleeAnimation();
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
                playerUnits[0].PlayFleeAnimation();
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

    public void ResetEnemyProgressor()
    {
        if (state != BattleState.Waiting) state = BattleState.Waiting;
        activeUnit = null;
        enemyProgressors[0].Slider.value = 0;
    }

    public void ResetPlayerProgressor()
    {
        if (state != BattleState.Waiting) state = BattleState.Waiting;
        activeUnit = null;
        playerProgressors[0].Slider.value = 0;

    }
}
