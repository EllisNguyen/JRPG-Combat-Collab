using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedIn : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.FadeIn();
    }
}
