using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject persistentObject;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (persistentObject == null)
        {
            persistentObject = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }
}
