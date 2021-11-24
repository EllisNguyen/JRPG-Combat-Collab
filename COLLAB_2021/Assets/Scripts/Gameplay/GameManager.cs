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
 
 */





public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance; // for easier accessibility
    public GameState currentGameState; //variable to hold enum value
    public static event Action<GameState> OnGameStateChanged; //Call an action so that other class can subscribe to and invoke the subcribed classes' functions with the passed in GameState

    [SerializeField] TopDownMovement r_topDownMovement; //TopDownMovement class reference

    private void Awake()
    {
        gameManagerInstance = this;

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //examples of handling
        switch (currentGameState)
        {
            case GameState.FreeRoam:
                //Do stuffs in FreeRoaming()
                FreeRoaming();
                break;
            case GameState.CombatPhase:
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
        }
    }


    /*public void UpdateState(GameState passedInState)
    {
        currentGameState = passedInState;


       

        OnGameStateChanged?.Invoke(passedInState);
        //Check if OnGameStateChanged is null or not.
        //if not then invoke the subcribed classes' functions with the passed in enum

    }*/

    private void InitCombat()
    {
        //do stuffs when combat
        //Debug.Log("3");
    }

    private void FreeRoaming()
    {

        //updates movements through Movements() in TopDownMovement class
        //Debug.Log("1");
        r_topDownMovement.Movements();

    }

    private void MenuScript()
    {
        //Menu stuffs go here
        //Debug.Log("2");
    }
    private void UpInventory()
    {

    }
}


//enum for game current state
public enum GameState
{
    FreeRoam,
    CombatPhase,
    Menu,
    UpInventory,

}
