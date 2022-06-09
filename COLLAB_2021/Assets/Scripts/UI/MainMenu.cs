///Author: Quan.TM
///Description: Main menu buttons function.
///Day created: 29/11/2021
///Last edited: DD/MM/YYYY - editor name.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject askQuit;
    public GameObject Menu;
   public void PlayGame()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

   public void QuitGame()
   {
        Menu.SetActive(false);
        askQuit.SetActive(true);
       Application.Quit();
   }
    public void ConfirmQuit()
    {

        Application.Quit();
    }
    public void RefuseQuit()
    {

        askQuit.SetActive(false);
        Menu.SetActive(true);
    }

}
