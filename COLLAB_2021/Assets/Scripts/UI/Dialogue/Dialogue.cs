using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Ly Duong Huy
 * Scripts: Dialogue
 * Object: N/A
 * Summary:
 * Handles dialogues
 */

//make it serializable and not dependant on MonoBehaviour so that it ios not required to be put on an object
[System.Serializable]
public class Dialogue
{
    //the name of the speaker
    public string speaker;


    //contents of the dialogue
    [TextArea(3,10)]
    public string[] sentences;
}
