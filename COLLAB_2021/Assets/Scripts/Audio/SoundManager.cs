///Author: Quan.TM
///Description: Sound manager.
///Day created: 01/12/2021
///Last edited: DD/MM/YYYY - editor name.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip example1, example2, example3;
    static AudioSource audioSource;
    void Start()
    {
        example1 = Resources.Load<AudioClip> ("example1");
        example2 = Resources.Load<AudioClip> ("example2");
        example2 = Resources.Load<AudioClip> ("example3");

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "example1":
                audioSource.PlayOneShot(example1);
                break;
            case "example2":
                audioSource.PlayOneShot(example2);
                break;
            case "example3":
                audioSource.PlayOneShot(example3);
                break;
        }
    }
}
