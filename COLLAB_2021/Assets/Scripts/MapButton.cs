using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapButton : MonoBehaviour, IPointerClickHandler
{
    public string sceneToLoad;

    public void OnPointerClick(PointerEventData eventData)
    {
        LoadScene.Instance.nextScene = sceneToLoad;
    }
}
