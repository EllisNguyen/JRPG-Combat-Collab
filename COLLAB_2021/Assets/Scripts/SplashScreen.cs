using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using DG.Tweening;

public class SplashScreen : MonoBehaviour
{
    public VideoPlayer videoplayer;
    public int sceneIndex;
    [SerializeField]  public CanvasGroup teamName;
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
        teamName.DOFade(1, 0.2f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}

