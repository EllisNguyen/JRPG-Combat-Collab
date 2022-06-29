///Author: Phap Nguyen.
///Description: Battle system for the game.
///Day created: 22/12/2022
///Last edited: 12/05/2022 - Phap Nguyen.

using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Sirenix.OdinInspector;

public enum BattleState { Start, Waiting, ActionSelection, MoveSelection, RunningTurn, Busy, Inventory, BattleOver }

public enum BattleAction { Move, SwitchCharacter, UseItem, Run, Wait }

[RequireComponent(typeof(BattleInfoGetter))]
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleState state;
    [SerializeField] BattleInfoGetter infoGetter;
    [SerializeField] CinemachineVirtualCamera vCamera;
    [SerializeField] float normalFov;
    [SerializeField] float critFov = 15f;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] Transform target;
    [SerializeField] AudioSource levelUpAudio;
    [FoldoutGroup("Camera targets")][SerializeField] Transform centerBattle;
    [FoldoutGroup("Camera targets")][SerializeField] Transform playerPos;
    [FoldoutGroup("Camera targets")][SerializeField] Transform enemyPos;
    [FoldoutGroup("Camera targets")][SerializeField] float shakeTimer;
    [FoldoutGroup("Camera targets")][SerializeField] float defaultTransitionTime = 0.35f;

    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    public Transform CenterBattle => centerBattle;
    public Transform PlayerPos => playerPos;
    public Transform EnemyPos => enemyPos;

    [FoldoutGroup("Player")] [SerializeField] List<BattleHud> playerHuds;
    [FoldoutGroup("Player")] [SerializeField] List<BattlePawn> playerUnits;
    [FoldoutGroup("Player")] [SerializeField] BattleHud[] pMemberHuds;
    [FoldoutGroup("Player")] [SerializeField] GameObject playerHudContainer;

    [PropertySpace(10)]

    [FoldoutGroup("Enemy")] [SerializeField] List<BattleHud> enemyHuds;
    [FoldoutGroup("Enemy")] [SerializeField] List<BattlePawn> enemyUnits;
    [FoldoutGroup("Enemy")][SerializeField] BattleHud[] eMemberHuds;
    [FoldoutGroup("Enemy")][SerializeField] GameObject enemyHudContainer;

    [PropertySpace(10)]

    [FoldoutGroup("Battle Components")] [SerializeField] BattlePawn playerPawn_Pref;
    [FoldoutGroup("Battle Components")] [SerializeField] BattlePawn enemyPawn_Pref;
    [FoldoutGroup("Battle Components")] [SerializeField] InventoryUI inventoryUI;
    [FoldoutGroup("Battle Components")] [SerializeField] BattleDialogue dialogueBox;
    [FoldoutGroup("Battle Components")] [SerializeField] MoveInfoPanel moveInfoPanel;

    public MoveInfoPanel MoveInfoPanel => moveInfoPanel;
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

    [FoldoutGroup("Spawnpoints")] public List<BattleSpawnPoint> playerSpawnPoints;
    [FoldoutGroup("Spawnpoints")] public List<BattleSpawnPoint> enemySpawnPoints;

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
    int escapeAttempt;
    MoveData moveToLearn;
    bool playerPerform = false;
    bool enemyPerform = false;

    public bool PlayerPerform { get { return playerPerform; } set { playerPerform = value; } }
    public bool EnemyPerform { get { return enemyPerform; } set { enemyPerform = value; } }

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
        infoGetter.battleSystem = this;
        brain = FindObjectOfType<CinemachineBrain>();

        target = centerBattle;
        normalFov = vCamera.m_Lens.FieldOfView;

        brain.m_DefaultBlend.m_Time = defaultTransitionTime;
        vCamera.Follow = target;
        vCamera.LookAt = target;
    }

    void SpawnPlayerPawn(CharacterParty party)
    {
        //Destroy all current child in all the spawnpoints.
        for (int i = 0; i < playerSpawnPoints.Count; i++)
        {        
            foreach (Transform child in playerSpawnPoints[i].transform)
            {
                Destroy(child.gameObject);
            }
        }

        playerUnits = new List<BattlePawn>();

        for (int j = 0; j < party.Characters.Count; j++)
        {
            BattlePawn unitObj = Instantiate(playerPawn_Pref, playerSpawnPoints[j].gameObject.transform);
            playerUnits.Add(unitObj);

            unitObj.name = "CHARACTER: " + party.Characters[j].Base.charName;

            foreach (BattleSpawnPoint spawnPoint in playerSpawnPoints)
            {
                if (playerUnits.Count == party.Characters.Count) break;
                if (spawnPoint.transform.childCount > 0)
                {
                    spawnPoint.SetActiveSpawnpoint(unitObj);
                }
            }
        }
    }

    void EnablePlayerHud(CharacterParty party)
    {
        //Find all BattleHud class in child and put in an array.
        pMemberHuds = playerHudContainer.GetComponentsInChildren<BattleHud>(true);

        //Debug.Log("PLAYER member slots = " + pMemberHuds.Length);

        for (int i = 0; i < pMemberHuds.Length; i++)
        {
            //Check if the party member is withing the number of characters.
            if (i < party.Characters.Count)
            {
                //Enable slot before setting the data.
                pMemberHuds[i].gameObject.SetActive(true);
            }
            else if (i > party.Characters.Count)
            {
                pMemberHuds[i].gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemyPawn(CharacterParty party)
    {
        //Destroy all current child in all the spawnpoints.
        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            foreach (Transform child in enemySpawnPoints[i].transform)
            {
                Destroy(child.gameObject);
            }
        }

        enemyUnits = new List<BattlePawn>();

        for (int j = 0; j < party.Characters.Count; j++)
        {
            BattlePawn unitObj = Instantiate(enemyPawn_Pref, enemySpawnPoints[j].gameObject.transform);
            enemyUnits.Add(unitObj);

            unitObj.name = "ENEMY: " + party.Characters[j].Base.charName;

            foreach (BattleSpawnPoint spawnPoint in enemySpawnPoints)
            {
                if (spawnPoint.transform.childCount > 0)
                {
                    spawnPoint.SetActiveSpawnpoint(unitObj);
                }
            }
        }
    }

    void EnableEnemyHud(CharacterParty party)
    {
        //Find all BattleHud class in child and put in an array.
        eMemberHuds = enemyHudContainer.GetComponentsInChildren<BattleHud>(true);

        //Debug.Log("ENEMY member slots = " + eMemberHuds.Length);

        for (int i = 0; i < eMemberHuds.Length; i++)
        {
            //Check if the party member is withing the number of characters.
            if (i < party.Characters.Count)
            {
                //Enable slot before setting the data.
                eMemberHuds[i].gameObject.SetActive(true);
            }
            else if (i > party.Characters.Count)
            {
                eMemberHuds[i].gameObject.SetActive(false);
            }
        }
    }

    public void StartBattle(CharacterParty _playerParty, CharacterParty _enemyParty)
    {
        this.playerParty = _playerParty;
        this.enemyParty = _enemyParty;

        vCamera.Priority = 11;

        CinemachineTransposer vTranposer = vCamera.AddCinemachineComponent<CinemachineTransposer>();
        
        float angle = 0;
        DOTween.To(() => angle, x => angle = x, -15, 1f)
            .OnUpdate(() => {
                vTranposer.m_FollowOffset = new Vector3(0, 1, angle);
            });

        CinemachineComposer vComposer = vCamera.AddCinemachineComponent<CinemachineComposer>();

        SpawnPlayerPawn(_playerParty);
        SpawnEnemyPawn(_enemyParty);

        StartCoroutine(SetupBattle());
    }

    /// <summary>
    /// Reset the speed progressor for everyone.
    /// </summary>
    void ResetSpeedProgressor()
    {
        activeCharacter.sprite = null;
        activeUnit = null;

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
            SpeedProgressor progresorObj = Instantiate(speedProgressorPrefab, progressorHolder.transform);
            progresorObj.SetProgressorData(playerUnit.Character, true);
            playerProgressors.Add(progresorObj);

            progresorObj.name = "PROGRESSOR: " + playerUnit.Character.Base.charName.ToUpper();
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {
            playerUnits[i].Progressor = playerProgressors[i];
        }

        //Add enemy progressor onto correct position.
        foreach (BattlePawn enemyUnit in enemyUnits)
        {
            var progresorObj = Instantiate(speedProgressorPrefab, progressorHolder.transform);
            progresorObj.SetProgressorData(enemyUnit.Character, false);
            enemyProgressors.Add(progresorObj);

            progresorObj.name = "PROGRESSOR: " + enemyUnit.Character.Base.name.ToUpper();

            for (int i = 0; i < enemyUnits.Count; i++)
            {
                enemyUnits[i].Progressor = progresorObj;
            }
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            enemyUnits[i].Progressor = enemyProgressors[i];
        }
    }

    public IEnumerator SetupBattle()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            playerUnits[i].Setup(playerParty.Characters[i]);

            //print("char num " + i + " with name of " + playerParty.Characters[i].Base.charName);

            playerUnits[i].Hud = playerHuds[i];
            playerHuds[i].SetData(playerUnits[i].Character);
        }

        for (int j = 0; j < enemyUnits.Count; j++)
        {
            enemyUnits[j].Setup(enemyParty.Characters[j]);
            enemyUnits[j].Hud = enemyHuds[j];
            enemyHuds[j].SetData(enemyUnits[j].Character);
            enemyHuds[j].DisableNonPlayerElement();
        }
        EnablePlayerHud(playerParty);
        EnableEnemyHud(enemyParty);

        ResetSpeedProgressor();

        if (enemyUnits.Count > 1)
        {
            yield return dialogueBox.TypeDialogue($"You stumbled upon a group of enemies led by {enemyUnits[0].Character.Base.charName.ToUpper()}.");
        }
        else if (enemyUnits.Count == 1)
        {
            yield return dialogueBox.TypeDialogue($"You stumbled upon enemy {enemyUnits[0].Character.Base.charName.ToUpper()}.");
        }

        yield return null;

        //dialogueBox.SetSkillList(playerUnits[0].Character.Moves);

        for (int k = 0; k < playerUnits.Count; k++)
        {
            playerUnits[k].Skills = playerUnits[0].Character.Moves;
        }

        //yield return new WaitForSecondsRealtime(1f);

        yield return dialogueBox.TypeDialogue($"Ready for action.");
        state = BattleState.Waiting;
    }

    public void BasicAttack()
    {
        var move = new Move(basicAttack);

        currentMove = move;

        if(activeUnit.IsPlayerUnit)
            StartCoroutine(RunTurnsPlayer(BattleAction.Move));
        else
            StartCoroutine(RunTurnsEnemy(BattleAction.Move));
    }

    public void BasicGuard()
    {
        var move = new Move(basicGuard);

        currentMove = move;

        if (activeUnit.IsPlayerUnit)
            StartCoroutine(RunTurnsPlayer(BattleAction.Move));
        else
            StartCoroutine(RunTurnsEnemy(BattleAction.Move));
    }

    public void SpawnParticle(Move move, BattlePawn target)
    {
        if (move.Base.HitEffect == null) return;
        
        Instantiate(move.Base.HitEffect, target.transform.position + move.Base.SpawnOffset, Quaternion.identity);
    }

    public void HandleUpdate()
    {
        switch (state)
        {
            case BattleState.Start:
                //WaitingForTurn();
                break;
            case BattleState.Waiting:
                WaitingForTurn();
                break;
            case BattleState.ActionSelection:
                break;
            case BattleState.MoveSelection:

                break;
            case BattleState.RunningTurn:
                if (playerPerform)
                    StartCoroutine(RunTurnsPlayer(BattleAction.Move));
                else if (enemyPerform)
                    StartCoroutine(RunTurnsEnemy(BattleAction.Move));
                break;
            case BattleState.Busy:
                //if(activeUnit.IsPlayerUnit)
                    ActionSelection();
                break;
            case BattleState.Inventory:

                break;
            case BattleState.BattleOver:
                //StartCoroutine(GameController.Instance.BattleEndSequence());
                break;
            default:
                break;
        }
    }

    IEnumerator CameraShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin multiChannelPerlin = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        multiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;

        while (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            yield return null;
        }

        if (shakeTimer <= 0)
        {
            multiChannelPerlin.m_AmplitudeGain = 0;
        }
    }

    void PlayerAction()
    {
        state = BattleState.ActionSelection;
        target = playerPos;

        dialogueBox.SetDialogue("Select your next move.");

        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableActionSelector(true);
    }

    void WaitingForTurn()
    {
        activeCharacter.sprite = null;
        activeUnit = null;

        dialogueBox.EnableDialogueText(false);

        if(activeUnit == null)
        {
            for (int i = 0; i < playerUnits.Count; i++)
            {
                StartCoroutine(playerUnits[i].Progressor.SpeedProgress(playerUnits[i].Character, playerUnits[i]));
                //print(playerUnits[i].Character.Speed);
            }

            for (int j = 0; j < enemyUnits.Count; j++)
            {
                StartCoroutine(enemyUnits[j].Progressor.SpeedProgress(enemyUnits[j].Character, enemyUnits[j]));
                //print(enemyUnits[j].Character.Speed);
            }
        }
    }

    IEnumerator HandleCharacterFainted(BattlePawn faintedUnit)
    {
        //Display enemy fainted dialogue and play faint animation.
        yield return dialogueBox.TypeDialogue($"{faintedUnit.Character.Base.charName.ToUpper()} has fallen.");
        faintedUnit.PlayFaintAnimation();

        //Enemy fainted and player won.
        yield return new WaitForSeconds(2f);

        //Calculate EXP gained if enemy fainted.
        if (!faintedUnit.IsPlayerUnit)
        {
            yield return dialogueBox.TypeDialogue($"You have slain enemy {faintedUnit.Character.Base.charName.ToUpper()}.");

            //EXP gains.
            var expYield = faintedUnit.Character.Base.expYield;
            int enemyLevel = faintedUnit.Character.Level;

            int expGain = Mathf.FloorToInt((expYield * enemyLevel) / 7);
            for (int i = 0; i < playerUnits.Count; i++)
            {
                playerUnits[i].Character.Exp += expGain;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            yield return dialogueBox.TypeDialogue($"Gained {expGain} exp.");

            for (int i = 0; i < playerParty.Characters.Count; i++)
            {
                yield return playerHuds[i].SetExpSmooth();
                
                //Check level up.
                //Update the HUD.
                //Learn new move(s).
                while (playerUnits[i].Character.CheckForLevelUp())
                {
                    for (int j = 0; j < playerParty.Characters.Count; j++)
                    {
                        playerHuds[i].SetLevel();
                    }
                
                    playerUnits[i].levelUp.Play();
                    playerUnits[i].PlayLevelUpAudio();
                    yield return dialogueBox.TypeDialogue($"{playerUnits[i].Character.Base.charName.ToUpper()} grew to level {playerUnits[i].Character.Level}.");

                    var newMove = playerUnits[0].Character.GetLearnableMoveAtCurrentLevel();

                    //Check if newMove return a null.
                    if (newMove != null)
                    {
                        //ADD A NEW MOVE TO THE CHARACTER MOVESET.
                        playerUnits[i].Character.LearnMove(newMove);

                        //Show dialogue and added to moves list.
                        yield return dialogueBox.TypeDialogue($"{playerUnits[i].Character.Base.charName.ToUpper()} learned {newMove.Base.Name.ToUpper()}.");
                    }

                    for (int k = 0; k < playerParty.Characters.Count; k++)
                    {
                        yield return playerHuds[i].SetExpSmooth();
                    }
                }
            }


            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return dialogueBox.TypeDialogue($"Damn bro u ded.");
        }

        CheckForBattleOver(faintedUnit);
    }

    //Check for the Unit that is fainted is Player's or Opponent's.
    void CheckForBattleOver(BattlePawn faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            //Declare a check for healthy creature in party.
            var nextCreature = playerParty.GetHealthyCharacters();

            BattleOver(false, true);
        }
        else
        {
            BattleOver(true, true);
        }
    }

    //
    public IEnumerator RunTurnsPlayer(BattleAction playerAction)
    {
        //Perform player's turn.
        state = BattleState.RunningTurn;

        action = playerAction;

        playerPerform = false;
        //Check if player perform a move.
        if (playerAction == BattleAction.Move)
        {
            if (currentMove.Mana > activeUnit.Character.MP)
            {
                dialogueBox.EnableActionSelector(false);
                dialogueBox.EnableDialogueText(true);
                yield return dialogueBox.TypeDialogue($"Not enough MP.");

                ActionSelection();
                yield break;
            }

            var target = currentMove.Base.Target;
            switch (target)
            {
                case MoveTarget.Foe:
                    yield return RunMove(activeUnit, enemyUnits[0], currentMove);
                    break;
                case MoveTarget.Self:
                    yield return RunMove(activeUnit, activeUnit, currentMove);
                    break;
                case MoveTarget.PlayerTeam:
                    //TODO: make AOE effect.
                    break;
                case MoveTarget.EnemyTeam:
                    //TODO: make AOE effect.
                    break;
                default:
                    break;
            }
            
            yield return RunAfterTurn(activeUnit);//End turn.

            if (state == BattleState.BattleOver) yield break;
        }
        else
        {
            if (playerAction == BattleAction.Run)
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
    }

    public IEnumerator RunTurnsEnemy(BattleAction enemyAction)
    {
        //Perform Enemy's turn.

        enemyPerform = false;
        
        if (enemyAction == BattleAction.Move)
        {
            //Randomize the enemy's move.
            Move enemyMove = activeUnit.Character.GetRandomMove(activeUnit.Character);
            var target = enemyMove.Base.Target;
            int randomTarget = UnityEngine.Random.Range(0, playerUnits.Count);

            switch (target)
            {
                case MoveTarget.Foe:
                    yield return RunMove(activeUnit, playerUnits[randomTarget], enemyMove);
                    break;
                case MoveTarget.Self:
                    yield return RunMove(activeUnit, activeUnit, enemyMove);
                    break;
                case MoveTarget.PlayerTeam:
                    //TODO: make AOE effect.
                    break;
                case MoveTarget.EnemyTeam:
                    //TODO: make AOE effect.
                    break;
                default:
                    break;
            }  

            yield return RunAfterTurn(activeUnit);//End turn.
            if (state == BattleState.BattleOver) yield break;
        }
    }

    public void ActionSelection()
    {
        dialogueBox.EnableDialogueText(false);
        if (activeUnit != null)
        {
            if (activeUnit.IsPlayerUnit)// == playerUnits[0])
            {
                vCamera.m_LookAt = playerPos;
                PlayerAction();
            }
            else if (!activeUnit.IsPlayerUnit)// == enemyUnits[0])
            {
                vCamera.m_LookAt = enemyPos;
                enemyPerform = true;
                state = BattleState.RunningTurn;
                //StartCoroutine(RunTurnsEnemy(BattleAction.Move));
                //ResetEnemyProgressor();
            }
        }
    }

    #region Performing move logic
    //Object oriented scripting toward performing the move of the creature.
    //
    IEnumerator RunMove(BattlePawn sourceUnit, BattlePawn targetUnit, Move move)
    {
        sourceUnit.Character.CurrentMove = move;

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

        sourceUnit.Character.DecreaseMP(move.Mana);
        sourceUnit.Hud.UpdateMP();

        vCamera.m_LookAt = centerBattle;

        //print($"Used {move.Base.Name.ToUpper()}.");
        yield return dialogueBox.TypeDialogue($"{sourceUnit.Character.Base.charName.ToUpper()} used {move.Base.Name.ToUpper()}.");
        
        //Check if the attack landed.
        if (CheckIfMoveHits(move, sourceUnit.Character, targetUnit.Character))
        {
            if(move.Base.Guard) sourceUnit.PlayGuardAnimation(move, sourceUnit);
            else
                sourceUnit.PlayAttackAnimation(move, targetUnit);

            SpawnParticle(move, targetUnit);

            if(move.Base.Target == MoveTarget.Foe)
            {
                targetUnit.PlayHitAnimation();//Hit animation.
                yield return CameraShake(4, 0.1f);
            }


            //Check if the move is Status effect.
            if (move.Base.Category == MoveCategory.Status)
            {   
                yield return RunMoveEffect(move.Base.Effect, sourceUnit.Character, targetUnit.Character, move.Base.Target);
            }
            //Do damage if not Status effect move.
            else
            {
                //Declare fainted boolean
                brain.m_DefaultBlend.m_Time = 15f;
                //brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
                var damageDetails = targetUnit.Character.TakeDamage(move, sourceUnit.Character, targetUnit.Character);

                if(damageDetails.Critical > 1)
                {
                    vCamera.m_LookAt = targetUnit.LookAtPos;
                    vCamera.m_Lens.FieldOfView = critFov;
                    Time.timeScale = 0.75f;
                    yield return new WaitForSecondsRealtime(0.5f);
                }

                //Show crit or type effectiveness dialogue.
                yield return ShowDamageDetails(damageDetails);

                Time.timeScale = 1f;
                //Call the update health bar func.
                yield return targetUnit.Hud.WaitForHpUpdate();

                brain.m_DefaultBlend.m_Time = defaultTransitionTime;
                brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
                vCamera.m_Lens.FieldOfView = normalFov;
                vCamera.m_LookAt = centerBattle;
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

    List<Character> Units(List<BattlePawn> targets)
    {
        List<Character> chars = new List<Character>();

        for (int i = 0; i < targets.Count; i++)
        {
            chars.Add(targets[i].Character);
        }

        return chars;
    }

    //IEnumerator RunAOEMove(BattlePawn sourceUnit, List<BattlePawn> targetUnits, Move move)
    //{
    //    sourceUnit.Character.CurrentMove = move;
    //    List<Character> targets = new List<Character>();

    //    if (move.Base.Target == MoveTarget.EnemyTeam)
    //    {
    //        targetUnits = enemyUnits;
    //    }
    //    else if (move.Base.Target == MoveTarget.PlayerTeam)
    //    {
    //        targetUnits = playerUnits;
    //    }
    //    targets = Units(targetUnits);

    //    //Declare a bool from the creature current status and check if any status will stop the creature move.
    //    bool canRunMove = sourceUnit.Character.OnBeforeMove();
    //    if (!canRunMove)
    //    {
    //        yield return ShowStatusChanges(sourceUnit.Character);

    //        //Call the update health bar function.
    //        yield return sourceUnit.Hud.WaitForHpUpdate();

    //        yield break;
    //    }
    //    yield return ShowStatusChanges(sourceUnit.Character);

    //    dialogueBox.EnableActionSelector(false);
    //    dialogueBox.EnableDialogueText(true);

    //    //Decrease the PP of the move and fire the dialogue coroutine.

    //    sourceUnit.Character.DecreaseMP(move.Mana);
    //    sourceUnit.Hud.UpdateMP();

    //    vCamera.m_LookAt = centerBattle;

    //    print($"Used {move.Base.Name.ToUpper()}.");
    //    yield return dialogueBox.TypeDialogue($"{sourceUnit.Character.Base.charName.ToUpper()} used {move.Base.Name.ToUpper()}.");

    //    //Check if the attack landed.
    //    if (CheckIfMoveHits(move, sourceUnit.Character, targets))
    //    {
    //        if (move.Base.Guard) sourceUnit.PlayGuardAnimation(move, sourceUnit);
    //        else
    //            sourceUnit.PlayAttackAnimation(move, targetUnits);

    //        SpawnParticle(move, targetUnits);

    //        if (move.Base.Target == MoveTarget.Foe)
    //        {
    //            targetUnits.PlayHitAnimation();//Hit animation.
    //            yield return CameraShake(4, 0.1f);
    //        }


    //        //Check if the move is Status effect.
    //        if (move.Base.Category == MoveCategory.Status)
    //        {
    //            yield return RunMoveEffect(move.Base.Effect, sourceUnit.Character, targetUnits.Character, move.Base.Target);
    //        }
    //        //Do damage if not Status effect move.
    //        else
    //        {
    //            //Declare fainted boolean
    //            brain.m_DefaultBlend.m_Time = 15f;
    //            //brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
    //            var damageDetails = targetUnits.Character.TakeDamage(move, sourceUnit.Character, targetUnits.Character);

    //            if (damageDetails.Critical > 1)
    //            {
    //                vCamera.m_LookAt = targetUnits.LookAtPos;
    //                vCamera.m_Lens.FieldOfView = critFov;
    //                Time.timeScale = 0.75f;
    //                yield return new WaitForSecondsRealtime(0.5f);
    //            }

    //            //Show crit or type effectiveness dialogue.
    //            yield return ShowDamageDetails(damageDetails);

    //            Time.timeScale = 1f;
    //            //Call the update health bar func.
    //            yield return targetUnits.Hud.WaitForHpUpdate();

    //            brain.m_DefaultBlend.m_Time = defaultTransitionTime;
    //            brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
    //            vCamera.m_Lens.FieldOfView = normalFov;
    //            vCamera.m_LookAt = centerBattle;
    //        }

    //        //Check for available secondary effects.
    //        //Check if secondary effect available.
    //        //Check for opponent health.
    //        if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnits.Character.HP > 0)
    //        {
    //            //Loop through all secondary effects.
    //            foreach (var secondary in move.Base.Secondaries)
    //            {
    //                //Chance of secondary fx to occur.
    //                var rnd = UnityEngine.Random.Range(1, 101);

    //                //Compare random number to fixed chance.
    //                if (rnd <= secondary.Chance)
    //                {
    //                    yield return RunMoveEffect(secondary, sourceUnit.Character, targetUnits.Character, secondary.Target);
    //                }
    //            }
    //        }

    //        //Check for the creature's health.
    //        if (targetUnits.Character.HP <= 0)
    //        {
    //            yield return HandleCharacterFainted(targetUnits);
    //        }
    //    }
    //    //If the move does not landed / missed.
    //    else
    //    {
    //        yield return dialogueBox.TypeDialogue($"{sourceUnit.Character.Base.charName.ToUpper()}'s attack missed.");
    //    }
    ////}

    //E
    IEnumerator RunMoveEffect(MoveEffect effects, Character source, Character target, MoveTarget moveTarget)
    {
        //STAT BOOSTING EFFECT.
        //Check if the move can actually do something.
        if (effects.Boost != null)
        {
            //Apply boost to source unit if target is SELF.
            if (moveTarget == MoveTarget.Self)
            {
                source.ApplyBoost(effects.Boost);
            }
            //Apply boost to source unit if target is FOE.
            else
            {
                //TODO: play VFX.
                target.ApplyBoost(effects.Boost);
            }
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

        ClearProgressor();
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
        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = source.StatBoosts[Stat.Evasion];

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
    IEnumerator SwitchCharacter(Character newCharacter, bool isTrainerAboutToUse = false)
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
        playerUnits[0].Setup(newCharacter);

        //
        //dialogueBox.SetMoveName(newCreature.Moves);

        //Set dialogue text with typing effect
        yield return (dialogueBox.TypeDialogue($"Do your best {newCharacter.Base.charName.ToUpper()}!"));

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
            dialogueBox.EnableDialogueText(true);
            //Dequeue the first message and store it in message var.
            var message = character.StatusChanges.Dequeue();

            //if(character.)

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
    void BattleOver(bool won, bool isOver)
    {
        state = BattleState.BattleOver;

        GameManager.Instance.FadeOut();

        for (int i = 0; i < playerUnits.Count; i++)
        {
            playerUnits[i].Hud.ClearData();
        }

        for (int j = 0; j < enemyUnits.Count; j++)
        {
            enemyUnits[j].Hud.ClearData();
        }

        //Loop through all character in party.
        //Reset all boosted stats.
        playerParty.Characters.ForEach(c => c.OnBattleOver());
        enemyParty.Characters.ForEach(e => e.OnBattleOver());

        //Notify the game that the battle is over.
        OnBattleOver(isOver);

        if (won) Destroy(enemy.gameObject);
        else enemy.gameObject.SetActive(true);
    }

    #region HANDLE ACTION SELECTION

    public void Attack()
    {
        BasicAttack();
    }

    public void Guard()
    {
        BasicGuard();
    }

    public void Skill()
    {
        dialogueBox.EnableSkillSelector(true);
        dialogueBox.SetSkillList(activeUnit.Character.Moves);
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
        dialogueBox.EnableDialogueText(true);

        ++escapeAttempt;

        //Get player's and enemy's speed.
        int playerSpeed = playerUnits[0].Character.Base.MaxSpeed * 4;
        int enemySpeed = enemyUnits[0].Character.Base.MinSpeed;

        if (enemySpeed < playerSpeed)
        {
            for (int i = 0; i < playerUnits.Count; i++)
            {
                playerUnits[i].PlayFleeAnimation();
            }
            yield return dialogueBox.TypeDialogue("Got away safely.");
            BattleOver(false, true);
        }
        else
        {
            //Battle escape calculation.
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeAttempt;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                for (int i = 0; i < playerUnits.Count; i++)
                {
                    playerUnits[i].PlayFleeAnimation();
                }
                yield return dialogueBox.TypeDialogue("Got away safely.");
                BattleOver(false, true);
            }
            //Continue battle when escape failed.
            else
            {
                yield return dialogueBox.TypeDialogue("Damn that thing's still on our tails.");
                state = BattleState.RunningTurn;
            }
        }

        dialogueBox.EnableDialogueText(false);
    }

    public void ResetEnemyProgressor()
    {
        if (state != BattleState.Waiting) state = BattleState.Waiting;
        activeUnit.Progressor.Slider.value = 0;
        activeUnit = null;
    }

    public void ResetPlayerProgressor()
    {
        if (state != BattleState.Waiting) state = BattleState.Waiting;
        activeUnit.Progressor.Slider.value = 0;
        activeUnit = null;
    }

    public void ClearProgressor()
    {
        activeUnit.Progressor.Slider.value = 0;
        activeUnit = null;

        if (state != BattleState.Waiting) state = BattleState.Waiting;

        //print("clear progress");
    }
}
