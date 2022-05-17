using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    //the quest questprfab
    [SerializeField] private GameObject questPrefab;
    //where the questprefab go
    [SerializeField] private Transform questsContent;
    //what is the gameHolder do
    [SerializeField] public GameObject questHolder;
    //THe list of current quest
    public List<Quest> CurrentQuests;
    private void Awake()
    {
        //search each quest in the CurrentQuest List
        foreach(var quest in CurrentQuests)
        {
            //if not clicked
            quest.HaveClicked = false;
            //initalize the quest
            //questHolder.SetActive(true);
            quest.Initialize();
            //add all the input to quest
            quest.QuestCompleted.AddListener(OnQuestCompleted);
            //instantiate the quest window
            GameObject questObj = Instantiate(questPrefab, questsContent);
            Debug.Log(questObj.name);
            //get the icon
            questObj.transform.Find("icon").GetComponent<Image>().sprite = quest.Infomation.Icon;
            //get the button click
            questObj.GetComponent<Button>().onClick.AddListener(delegate
            {
                if (quest.HaveClicked == false)
                {
                    
                    //get the quest window and initalize the quest
                    questHolder.GetComponent<QuestWindow>().Initalize(quest);
                    //make the quest holder to true
                    questHolder.SetActive(true);
                    //set the have click to true then no one can click
                    quest.HaveClicked = true;
                }
            });
        }
    }
    public void CreateQuest()
    {
        //search each quest in the CurrentQuest List
        foreach (var quest in CurrentQuests)
        {
            //if not clicked
            quest.HaveClicked = false;
            //initalize the quest
            //questHolder.SetActive(true);
            quest.Initialize();
            //add all the input to quest
            quest.QuestCompleted.AddListener(OnQuestCompleted);
            //instantiate the quest window
            GameObject questObj = Instantiate(questPrefab, questsContent);
            Debug.Log(questObj.name);
            //get the icon
            questObj.transform.Find("icon").GetComponent<Image>().sprite = quest.Infomation.Icon;
            //get the button click
            questObj.GetComponent<Button>().onClick.AddListener(delegate
            {
                if (quest.HaveClicked == false)
                {
                    //get the quest window and initalize the quest
                    questHolder.GetComponent<QuestWindow>().Initalize(quest);
                    //make the quest holder to true
                    questHolder.SetActive(true);
                    //set the have click to true then no one can click
                    quest.HaveClicked = true;
                }
            });
        }
    }
    public void Build()
    {

    }
    private void OnQuestCompleted(Quest quest)
    {
        Debug.Log(CurrentQuests.IndexOf(quest));
        //questsContent.GetChild(CurrentQuests.IndexOf(quest)).Find("CheckMark").gameObject.SetActive(true);
        
    }

}
 