using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapUi : MonoBehaviour
{

    public TextMeshProUGUI mapName;
    public TextMeshProUGUI mapDescription;
    public GameObject expeditionButton;

    private void Start()
    {
        ClearLocationInfo();
    }

    public void SetLocationInfo(string name, string description, bool canFastTravel = false)
    {
        mapName.text = name;
        mapDescription.text = description;
        expeditionButton.SetActive(canFastTravel);
    }

    public void ClearLocationInfo()
    {
        mapName.text = "Location Empty";
        mapDescription.text = "Location Empty";
        expeditionButton.SetActive(false);
    }
}
