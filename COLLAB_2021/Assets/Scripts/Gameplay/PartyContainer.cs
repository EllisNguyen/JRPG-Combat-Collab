///Author: Phap Nguyen.
///Description: Container of player's party, control the UI elements and character's information.
///Day created: 22/03/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyContainer : MonoBehaviour
{
    public CharacterPanel[] memberSlots;//Declare the list of CharacterPanel.
    List<Character> characters;//Ref the list of current Character.

    CharacterParty party;//Ref the list of active character.

    void OnEnable()
    {
        Init();
    }

    void OnDisable()
    {
        //Loop through all member slots
        for (int i = 0; i < memberSlots.Length; i++)
        {
            memberSlots[i].ResetAppearAnim();
        }
    }

    /// <summary>
    /// Get the objects with CharacterPanel component.
    /// Set the party data according to the number of active character.
    /// Loop through all the CharacterPanel and set its UI.
    /// </summary>
    public void Init()
    {
        //Find all PartyMember_UI class in child and put in an array.
        memberSlots = GetComponentsInChildren<CharacterPanel>(true);

        //TODO: open GetPlayerPartyComponent() and refactor the TODO.
        party = CharacterParty.GetPlayerPartyComponent();

        //Set data, duh.
        SetPartyData();

        //Loop through the list of CharacterPanel and setup UI for all of them.
        foreach (var member in memberSlots)
        {
            member.SetUi();
        }

        //Everytime party is updated, run the SetPartyData function.
        party.OnUpdatedParty += SetPartyData;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetPartyData()
    {
        //StartCoroutine(SetPartyDataAsync());

        //Get the list of character player own from PlayerParty.
        characters = party.Characters;

        //Loop through all member slots
        for (int i = 0; i < memberSlots.Length; i++)
        {
            //Check if the party member is withing the number of creatures.
            if (i < characters.Count)
            {
                memberSlots[i].ResetAppearAnim();
                //Set data of the character correctly at i position.
                memberSlots[i].Init(characters[i]);
                StartCoroutine(memberSlots[i].EnableAnim());
            }
            else
            {
                //Disable slot if no character available at that slot.
                memberSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
