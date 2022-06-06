using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour, ISceneSetting
{
    [SerializeField] List<SceneDetails> connectedScenes;
    public bool IsLoaded { get; private set; }

    List<SavableEntity> savableEntities;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LoadScene();
            GameController.Instance.SetCurrentScene(this);

            print(GameController.Instance.CurrentScene);

            //Load all connected scene.
            foreach (var scene in connectedScenes)
            {
                scene.LoadScene();
            }

            //Unload scenes that are not connected.
            var prevScene = GameController.Instance.PrevScene;
            if (prevScene != null)
            {
                var previouslyLoadedScene = GameController.Instance.PrevScene.connectedScenes;

                foreach (var scene in previouslyLoadedScene)
                {
                    //Check if scene is NOT in list of currently loaded scenes.
                    if (!connectedScenes.Contains(scene) && scene != this)
                    {
                        //Unload it.
                        scene.UnloadScene();
                    }
                }
                if (!connectedScenes.Contains(prevScene))
                    prevScene.UnloadScene();
            }
        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            var operation = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;

            //Ony check and load scene states when scene loaded complete.
            operation.completed += (AsyncOperation op) =>
            {
                savableEntities = GetSavableEntitiesScene();
                SavingSystem.i.RestoreEntityStates(savableEntities);
            };
        }
    }

    public void UnloadScene()
    {
        if (IsLoaded)
        {
            SavingSystem.i.CaptureEntityStates(savableEntities);

            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }

    List<SavableEntity> GetSavableEntitiesScene()
    {
        var curScene = SceneManager.GetSceneByName(gameObject.name);

        //Find all SavableEntity belong in the scene.
        var savableEntities = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == curScene).ToList();

        return savableEntities;
    }

    #region Audio control
    //TODO: control the list of creature will be spawn in this area.
    //TODO: reference the BgmHandler to loop the track, and play the correct audio track.
    //TODO: 
    [Header("Properties")]
    public string mapName;
    public Sprite mapNameBoxTexture;

    [Header("Audio settings")]
    public AudioClip mapBGMClip;
    public int mapBGMLoopStartSamples = 0;
    public AudioClip mapBGMNightClip = null;
    public int mapBGMNightLoopStartSamples = 0;

    public void OnChangeArea()
    {

    }

    // returns the BGM to an external script
    public AudioClip getBGM()
    {
        if (mapBGMNightClip != null)
        {
            float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);
            if (time >= 20 || time < 3.5f)
            {
                //night
                return mapBGMNightClip;
            }
        }
        return mapBGMClip;
    }

    public int getBGMLoopStartSamples()
    {
        if (mapBGMNightClip != null)
        {
            float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);
            if (time >= 20 || time < 3.5f)
            {
                //night
                return mapBGMNightLoopStartSamples;
            }
        }
        return mapBGMLoopStartSamples;
    }
    #endregion

}
