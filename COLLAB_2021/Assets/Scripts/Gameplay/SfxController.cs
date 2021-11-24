using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{
    public static SfxController sfx { get; private set; }

    private void Awake()
    {
        sfx = this;
    }

    [SerializeField] AudioClip buttonSfx;
    [SerializeField] AudioClip selectSfx;


    public AudioClip ButtonSFX { get { return buttonSfx; } }
    public AudioClip SelectSFX { get { return selectSfx; } }
}
