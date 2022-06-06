using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDB
{
    //Dictionary to store all creature data using Key string and value of the creatures.
    static Dictionary<string, CharacterBaseStats> characters;

    public static void Init()
    {
        //Declare the dictionary.
        characters = new Dictionary<string, CharacterBaseStats>();

        //Find all CreatureData class in Resources directory and load them into the Dictionary.
        var characterArray = Resources.LoadAll<CharacterBaseStats>("");

        //Loop through all creature data.
        foreach (var character in characterArray)
        {
            //Check for similar key before adding data to the dictionary.
            if (characters.ContainsKey(character.charName))
            {
                Debug.LogError($"There are multiple creature with the name {character.charName}");
                continue;
            }

            characters[character.charName] = character;
        }
    }

    public static CharacterBaseStats GetCharacterByName(string name)
    {
        if (!characters.ContainsKey(name))
        {
            Debug.LogError($"{name} not found in the database.");
            return null;
        }

        return characters[name];
    }
}