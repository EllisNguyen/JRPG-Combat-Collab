///Author: Phab Nguyen.
///Description: this script do something.
///Day created: 20/01/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SHOULD BE ON THE SAME GAMEOBJECT WITH PLAYERCONTROLLER.
public class CharacterParty : MonoBehaviour
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
            character.Init();
        }
    }

    public List<Character> GetHealthyCharacters()
    {
        //Loop through list of characters and find the first one satisfy the HP>0 condition.
        var healthyCharacters = characters.Where(x => x.HP > 0).ToList();

        return healthyCharacters;
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

    public static CharacterParty GetPlayerPartyComponent()
    {
        //TODO: return the CharacterParty from player's prefab instead of from GameManager.
        return FindObjectOfType<PlayerEntity>().GetComponent<CharacterParty>();
    }

}
