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

        private class AudioJob
        {
            public AudioAction action;
            public AudioType type;

            public AudioJob(AudioAction _action, AudioType _type)
            {
                action = _action;
                type = _type;
            }
        }

        private enum AudioAction
        {
            START,
            STOP,
            RESTART
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
            public void PlayAudio(AudioType _type)
            {
                AddJob(new AudioJob(AudioAction.START, _type));
            }

            public void StopAudio(AudioType _type)
            {
                AddJob(new AudioJob(AudioAction.STOP, _type));
            }

            public void RestartAudio(AudioType _type)
            {
                AddJob(new AudioJob(AudioAction.RESTART, _type));
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
                foreach(AudioTrack track in tracks)
                {
                    foreach(AudioObject obj in track.audio)
                    {
                        //do not duplicate keys
                        if(audioTable.ContainsKey(obj.type))
                        {
                            LogWarning("You are trying to register audio [" + obj.type + "] that has already been registered.");
                        }
                        else
                        {
                            audioTable.Add(obj.type, track);
                            Log("Registering audio [" + obj.type + "].");
                        }
                    }
                }
            }

            private IEnumerator RunAudioJob(AudioJob job)
            {
            AudioTrack track = (AudioType)AudioTable[job.type];
            }
            
            private void AddJob(AudioJob job)
            {
                //remove conflicting jobs
                RemoveConflictingJobs(job.type);

                //start jobs
                IEnumerator jobRunner = RunAudioJob(job);
                jobTable.Add(job.type, jobRunner);
                Log("Starting job on [" + job.type + "] with operation: " + job.action);
            }

            private void RemoveJobs(AudioType type)
            {
                if(!jobTable.ContainsKey(type))
                {
                    LogWarning("Trying to stop a job [" + type + "] that is not running.");
                    return;
                }

                IEnumerator runningJob = (IEnumerator)jobTable[type];
                StopCoroutine(runningJob);
                jobTable.Remove(type);
            }

            private void RemoveConflictingJobs(AudioType type)
            {
                if(jobTable.ContainsKey(type))
                {
                    RemoveJobs(type);
                }

                AudioType conflictAudio = AudioType.None;
                foreach(DictionaryEntry entry in jobTable)
                {
                    AudioType audioType = (AudioType)entry.Key;
                    AudioTrack audioTrackInUse = (AudioTrack)audioTable[audioType];
                    AudioTrack audioTrackNeeded = (AudioTrack)audioTable[type];
                    if(audioTrackNeeded.source == audioTrackInUse.source)
                    {
                    //conflict
                        conflictAudio = audioType;
                    }
                }

                if(conflictAudio != AudioType.None)
                {
                    RemoveJobs(conflictAudio);
                }
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

    

