using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform goalsContent;
    [SerializeField] private Text xpText;
    
   

    public void Initalize(Quest quest)
    {
        Debug.Log(quest.name);
        titleText.text = quest.Infomation.Name;
        descriptionText.text = quest.Infomation.Description;
        gameObject.SetActive(true);

        foreach (var goal in quest.Goals)
        {
            
            GameObject goalObj = Instantiate(goalPrefab, goalsContent);
            //goalObj.transform.Find("Text").GetComponent<Text>().text = goal.GetDescription();
            GameObject countObj = goalObj.transform.Find("Count").gameObject;
            GameObject skipObj = goalObj.transform.Find("Skip").gameObject;
            if (goal.Completed)
            {
                countObj.SetActive(false);
                skipObj.SetActive(false);
                goalObj.transform.Find("Done").gameObject.SetActive(true);
            }
            else
            {
                countObj.GetComponent<Text>().text = goal.CurrentAmount + "/" + goal.RequiredAmount;
                skipObj.GetComponent<Button>().onClick.AddListener(delegate
                {
                    Debug.Log("zxc");
                    goal.Skip();
                    countObj.SetActive(true);
                    skipObj.SetActive(false);
                    goalObj.transform.Find("Done").gameObject.SetActive(true);
                });
            }
        }
        xpText.text = quest.Reward.XP.ToString();

    }
    public void CloseWindow()
    {
        gameObject.SetActive(false);
        for(int i = 0; i< goalsContent.childCount;i++)
        {
            Destroy(goalsContent.GetChild(i).gameObject);
        }
    }

}
