using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class BattleDialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject skillSelector;
    [SerializeField] GameObject skillContainer;
    [SerializeField] List<Move> skillsList;
    [SerializeField] List<SkillPanel> skillPanels;

    [SerializeField] SkillPanel skillPrefab;

    public void SetDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (var letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f/GameManager.Instance.letterPerSecond);
        }
    }

    public void EnableDialogueText(bool enabled)
    {
        dialogueText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableSkillSelector(bool enabled)
    {
        skillSelector.SetActive(enabled);
    }

    public void SetSkillList(List<Move> moves)
    {

        //Clear all unrelated item in the item list.
        foreach (Transform child in skillContainer.transform)
        {
            //Destroy them all.
            Destroy(child.gameObject);
        }

        skillsList = moves;

        foreach (var skill in skillsList)
        {
            var skillButton = Instantiate(skillPrefab, skillContainer.transform);

            skillButton.SetData(skill);
            //skillsList.Add(skill);
            //skillPanels.Add(skillButton);
        }

    }
}
