///Author: Quan.TM
///Description: Handle the audio control.
///Day created: 16/05/2022
///Last edited: DD/MM/YYYY - editor name.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController instance;

        public bool debug;
        public AudioTrack[] tracks;

        private Hashtable audioTable; //relationship between audio types(key) and audio tracks(value)
        private Hashtable jobTable; //relationship between audio types(key) and jobs(value) (Coroutine,IEnumerator)

        [System.Serializable]
        public class AudioObject
        {
            public AudioType type;
            public AudioClip clip;
        }

        [System.Serializable]
        public class AudioTrack
        {
            public AudioSource source;
            public AudioObject[] audio;
        }
        #region Unity Functions
            private void Awake() 
            {
                //instance
                if(!instance)
                {
                    Configure();
                }
            }

            private void OnDisable()
            {
                Dispose();
            }

        #endregion

        #region Public Functions
            public void PlayAudio(AudioType type)
            {

            }

            public void StopAudio(AudioType type)
            {
                
            }

            public void RestartAudio(AudioType type)
            {
                
            }
        #endregion

        #region Private Functions
            private void Configure()
            {
                instance = this;
                audioTable = new Hashtable();
                jobTable = new Hashtable();
                GenerateAudioTable();
            }

            private void Dispose()
            {

            }

            private void GenerateAudioTable()
            {

            }

            private void Log(string msg)
            {
                if(!debug) return;
                Debug.Log("[Audio Controller]: " + msg);
            }

            private void LogWarning(string msg)
            {
                if(!debug) return;
                Debug.LogWarning("[Audio Controller]: " + msg);
            }
        #endregion
    }
}

    

