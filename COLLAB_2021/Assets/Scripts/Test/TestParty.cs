using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestParty : MonoBehaviour
{
    Character _character;
    [SerializeField] List<Character> characters;
    [SerializeField] List<TestProgressor> progressors;

    public TestProgressor activeUnit;

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

    private void Update()
    {
        if (activeUnit == null)
        {
            for (int i = 0; i < progressors.Count; i++)
            {
                progressors[i].DoProgress(characters[i]);
            }
        }
    }

    [Button]
    public void ClearActive()
    {
        activeUnit.ResetSlider();
        activeUnit = null;
    }
}
