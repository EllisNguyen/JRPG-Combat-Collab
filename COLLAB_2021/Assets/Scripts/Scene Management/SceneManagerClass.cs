using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Author: Ly Duong Huy
 * Date: 21/04/2022
 * Class: SceneTransition
 * Object: SceneManager
 * Summary: Handles the transitions from one screen to another on Free roam.
 * Reference: Tarodev, (2021). Scene Manager - Load between scenes and show a progress bar - [ Unity Tutorial ][online]. Available from: https://www.youtube.com/watch?v=OmobsXZSRKo [accessed 21/04/2022].
 */

public class SceneManagerClass : MonoBehaviour
{
    //Variables
    public static SceneManagerClass instance; //easier access

    //Played once before starting the scene
    private void Awake()
    {
        //Check for the availability of the SceneTransition class
        if (instance == null)
        {
            //if not then assign this class as the SceneTransition and prevent it from being destroyed
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else //if another instance is present then destroy this.
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// LoadScene according to the argument. async is used in order to prepare for possibility of loading screen.
    /// </summary>
    /// <param name="sceneName"></param>
    public async void LoadScene(string sceneName)
    {
        //assigns the scene to the scene in the argument
        var scene = SceneManager.LoadSceneAsync(sceneName);
        //allow the scene to be activated
        scene.allowSceneActivation = true;

    }


}
