///Author: Phab Nguyen.
///Description: this script do something.
///Day created: 09/11/2021
///Last edited: 12/11/2021 - Phab Nguyen.

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    [SerializeField] List<CharacterBaseStats> characters;

    public event Action OnUpdatedParty;

    //Property to expose character list.
    public List<CharacterBaseStats> Characters
    {
        get
        {
            return characters;
        }
        set
        {
            //Set value of the creature and invoke the updated UI.
            characters = value;
            OnUpdatedParty?.Invoke();
        }
    }

    private void Awake()
    {
        //Loop through each creature in the party (creatures list)
        foreach (var character in characters)
        {
            //Init character stats number stuff.
            //character.Init();
        }
    }

    public CharacterBaseStats GetHealthyCharacter()
    {
        //Loop through list of characters and find the first one satisfy the HP>0 condition.
        return characters.Where(x => x.health > 0).FirstOrDefault();
    }

    public void AddCcharacter(CharacterBaseStats newCharacter)
    {
        if (characters.Count < 6)
        {
            characters.Add(newCharacter);

            //Invoke update creature party UI.
            OnUpdatedParty?.Invoke();
        }
        else
        {
            //TODO: transfer the creature to inventory.
        }
    }

}
