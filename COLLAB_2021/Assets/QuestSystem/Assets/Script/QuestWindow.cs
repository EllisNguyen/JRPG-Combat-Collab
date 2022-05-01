using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    //title text
    [SerializeField] private Text titleText;
    //Description Text
    [SerializeField] private Text descriptionText;
    //goal prefab
    [SerializeField] private GameObject goalPrefab;
    //where the goals content go
    [SerializeField] private Transform goalsContent;
    //where the xp text
    [SerializeField] private Text xpText;
    
   

    public void Initalize(Quest quest)
    {
        Debug.Log(quest.name);
        //get the title text
        titleText.text = quest.Infomation.Name;
        //get the description text
        descriptionText.text = quest.Infomation.Description;
        //set gameobject to true
        gameObject.SetActive(true);
        //loop the goal inside the quest goal
        foreach (var goal in quest.Goals)
        {
            //instantiate goal obj
            GameObject goalObj = Instantiate(goalPrefab, goalsContent);
            //goalObj.transform.Find("Text").GetComponent<Text>().text = goal.GetDescription();
            //find   countObj
            GameObject countObj = goalObj.transform.Find("Count").gameObject;
            //find skil obj
            GameObject skipObj = goalObj.transform.Find("Skip").gameObject;
            if (goal.Completed)
            {
                //set count object to false 
                countObj.SetActive(false);
                //set skip object to false 
                skipObj.SetActive(false);
                //get done gameobject
                goalObj.transform.Find("Done").gameObject.SetActive(true);
            }
            else
            {
                //get current amount
                countObj.GetComponent<Text>().text = goal.CurrentAmount + "/" + goal.RequiredAmount;
                //if  player click skip obj
                skipObj.GetComponent<Button>().onClick.AddListener(delegate
                {
                    Debug.Log("zxc");
                    //call skip function
                    goal.Skip();
                    //set countobj to true
                    countObj.SetActive(true);
                    //set skipobj to false
                    skipObj.SetActive(false);
                    //get done tick gameobject
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
