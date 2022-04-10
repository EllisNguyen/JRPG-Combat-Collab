///Author: Phab Nguyen.
///Description: this script do something.
///Day created: 09/11/2021
///Last edited: 12/11/2021 - Phab Nguyen.

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    Character _character;
    [SerializeField] List<Character> characters;

    public event Action OnUpdatedParty;

    //Property to expose character list.
    public List<Character> Characters
    {
        get
        {
            return characters;
        }
        set
        {
            //Set value of the character and invoke the updated UI.
            characters = value;
            OnUpdatedParty?.Invoke();
        }
    }

    private void Awake()
    {
        //Loop through each character in the party (character list)
        foreach (var character in characters)
        {
            //Init character stats number stuff.
            //character.Init();
        }
    }

    public Character GetHealthyCharacter()
    {
        //Loop through list of characters and find the first one satisfy the HP>0 condition.
        return characters.Where(x => x.HP > 0).FirstOrDefault();
    }

    public void AddCharacter(Character newCharacter)
    {
        if (characters.Count < 6)
        {
            characters.Add(newCharacter);

            //Invoke update character party UI.
            OnUpdatedParty?.Invoke();
        }
    }

    public static PlayerParty GetPlayerPartyComponent()
    {
        return FindObjectOfType<GameManager>().GetComponent<PlayerParty>();
    }

}
