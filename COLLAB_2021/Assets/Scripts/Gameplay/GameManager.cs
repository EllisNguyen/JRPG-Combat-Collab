using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Class: GameManager
Date: 20/10/2021
Author: Ly Duong Huy
Object: GameManager
Summary:
Handles the changes of the game state.
References:
Tarodev, (2021). Game Manager - Controlling the flow of your game [Unity Tutorial][online]. Available from: https://www.youtube.com/watch?v=4I0vonyqMi8 [accessed 20th October 2021]
 
Last edit: Nguyen Tien Phap - 20/02/2022

 */


public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // for easier accessibility
    public GameState gameState; //variable to hold enum value
    public static event Action<GameState> OnGameStateChanged; //Call an action so that other class can subscribe to and invoke the subcribed classes' functions with the passed in GameState

    public MapUi mapUi;

    [Header("Party setting")]
    public CharacterAboutPanel characterAbout;

    [Header("Inventory settings")]
    public int inventoryCapacity = 45;
    public Color belowCapacity;
    public Color aboveCapacity;
    public InventoryContainer inventoryContainer;
    public ItemAboutPanel itemAbout;
    public Color common;
    public Color uncommon;
    public Color rare;
    public Color legendary;
    public Color exotic;

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //examples of handling
        switch (gameState)
        {
            case GameState.FreeRoam:
                //Do stuffs in FreeRoaming()
                FreeRoaming();
                break;
            case GameState.Battle:
                //Do stuffs in InitCombat()
                InitCombat();
                break;
            case GameState.Menu:
                //Do stuffs in MenuScript()
                MenuScript();
                break;
            case GameState.UpInventory:
                UpInventory();
                break;
            case GameState.Paused:
                GamePaused();
                break;
        }
    }


    /*public void UpdateState(GameState passedInState)
    {
        currentGameState = passedInState;


       

        OnGameStateChanged?.Invoke(passedInState);
        //Check if OnGameStateChanged is null or not.
        //if not then invoke the subcribed classes' functions with the passed in enum

    }*/

    private void FreeRoaming()
    {

        //updates movements through Movements() in TopDownMovement class
        //Debug.Log("1");

    }

    private void InitCombat()
    {

        //updates movements through Movements() in TopDownMovement class
        //Debug.Log("1");

    }

    private void MenuScript()
    {
        //Menu stuffs go here
        //Debug.Log("2");
    }
    private void UpInventory()
    {

    }

    private void GamePaused()
    {
        
    }
}


//enum for game current state
public enum GameState
{
    FreeRoam,
    Battle,
    Menu,
    UpInventory,
    Paused,

}
