using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioButtonSound : MonoBehaviour
    {
        public AudioController audioController;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void onHover()
        {
            audioController.PlayAudio(AudioType.SFX_01);

        }
        public void onClicked()
        {
            audioController.PlayAudio(AudioType.SFX_02);

        }
    }
}
