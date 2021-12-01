///Author: Quan.TM
///Description: Keep the audio through scenes.
///Day created: 01/12/2021
///Last edited: DD/MM/YYYY - editor name.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioKeep : MonoBehaviour
{
    private static AudioKeep instance = null;
    public static AudioKeep Instance
    {
        get { return instance; }
    }
   void Awake() 
   {
       if(instance != null && instance != this)
       {
           Destroy(this.gameObject);
           return;
       }
       else
       {
           instance = this;
       }
       DontDestroyOnLoad(transform.gameObject);
   }
}
