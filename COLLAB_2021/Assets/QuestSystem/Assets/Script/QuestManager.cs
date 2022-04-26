using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questsContent;
    [SerializeField] public GameObject questHolder;
    public List<Quest> CurrentQuests;
    private void Awake()
    {
        
        foreach(var quest in CurrentQuests)
        {
            quest.HaveClicked = false;
            //questHolder.SetActive(true);
            quest.Initialize();

            quest.QuestCompleted.AddListener(OnQuestCompleted);
            GameObject questObj = Instantiate(questPrefab, questsContent);
            Debug.Log(questObj.name);
            questObj.transform.Find("icon").GetComponent<Image>().sprite = quest.Infomation.Icon;

            questObj.GetComponent<Button>().onClick.AddListener(delegate
            {
                if (quest.HaveClicked == false)
                {
                    questHolder.GetComponent<QuestWindow>().Initalize(quest);
                    questHolder.SetActive(true);
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
 