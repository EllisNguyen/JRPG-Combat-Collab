using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Class: PauseMenu
Date: 20/10/2021
Author: Ly Duong Huy
Summary:
Pause Menu with simple functions
*/
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;//store the pause menu game object
     public Scene sceneMainMenu; //store the main menu scene for return
    // Start is called before the first frame update

    //start the scene with pause menu being hidden
    void Start()
    {
        pauseMenu.SetActive(false);
        sceneMainMenu = SceneManager.GetSceneByName("Test Menu");
    }

    // Update is called once per frame
    void Update()
    {
        //Enable the pause menu if esc is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Time.timeScale = 0f; //Set time to paused
            pauseMenu.SetActive(true);
        }
    }

    //Resume to the game. Disable the Pause Menu, return the time scale to 1.
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Save()
    {
        //code for Save here
    }
    public void Load()
    {
        //code for Load here
    }

    //Quit to MainMenu using the index of the scene passed in sceneMainMenu
    public void ReturnMainMenu()
    {
        SceneManager.GetSceneByName("Test Menu");
       // SceneManager.LoadScene(sceneMainMenu.buildIndex, LoadSceneMode.Single);
        SceneManager.LoadScene("TestMenu", LoadSceneMode.Single);
        //Time.timeScale = 1f;
    }

    //Close and exit application
    public void ToDesktop()
    {
        Application.Quit();
    }

}
