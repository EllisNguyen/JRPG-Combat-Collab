using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class LoadScene : MonoBehaviour
{
    public string curScene;
    public string nextScene;

    public TextMeshProUGUI errorText;

    public static LoadScene Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        curScene = SceneManager.GetActiveScene().name;
    }

    public void LoadMapname()
    {
        if(nextScene == curScene)
        {
            StartCoroutine(GiveError());
            return;
        }
        else
        {
            GameManager.Instance.FadeOut();
            PlayerEntity.Player.DeactivateMenu();
            GameManager.Instance.gameState = GameState.FreeRoam;
            SceneManager.LoadScene(nextScene);
        }
    }

    IEnumerator GiveError()
    {
        Sequence errorSequence = DOTween.Sequence();

        errorSequence.Append(errorText.DOFade(1, 1));

        yield return new WaitForSeconds(2f);

        errorSequence.Append(errorText.DOFade(0, 1));
    }
}
