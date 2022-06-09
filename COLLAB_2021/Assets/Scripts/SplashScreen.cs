using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    public VideoPlayer videoplayer;
    public int sceneIndex;

    void Start()
    {
        videoplayer = GetComponentInParent<VideoPlayer>();
        StartCoroutine(WaitforVideoEnd());
    }

    private void Update()
    {
      
    }

    IEnumerator WaitforVideoEnd()
    {
        float videolength = (float)videoplayer.length;
        Debug.Log((float)videoplayer.length);
        yield return new WaitForSeconds(videolength);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}

