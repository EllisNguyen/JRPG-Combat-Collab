///Author: Quan.TM
///Description: Test the system.
///Day created: 16/05/2022
///Last edited: DD/MM/YYYY - editor name.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioTest : MonoBehaviour
    {
        public AudioController audioController;

        #region Unity Functions
#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.T))
            {
                audioController.PlayAudio(AudioType.ST_01);
            }
            if (Input.GetKeyUp(KeyCode.Y))
            {
                audioController.StopAudio(AudioType.ST_01);
            }
            if (Input.GetKeyUp(KeyCode.U))
            {
                audioController.RestartAudio(AudioType.ST_01);
            }

            if (Input.GetKeyUp(KeyCode.G))
            {
                audioController.PlayAudio(AudioType.SFX_01);
            }
            if (Input.GetKeyUp(KeyCode.H))
            {
                audioController.StopAudio(AudioType.SFX_01);
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                audioController.RestartAudio(AudioType.SFX_01);
            }
        }
#endif
        #endregion
    }

}

