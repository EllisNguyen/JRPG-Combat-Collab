using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MapLocationUi : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string locationName;
    [TextArea] public string locationDescription;
    MapUi mapUI;
    [SerializeField] bool canFastTravel = false;

    void Start()
    {
        mapUI = GameManager.Instance.mapUi;
    }

    public UnityEvent mouseOverEvent;
    public UnityEvent mouseExitEvent;
    public UnityEvent onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        mapUI.SetLocationInfo(locationName, locationDescription, canFastTravel);
        onClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mapUI.SetLocationInfo(locationName, locationDescription);
        mouseOverEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        mouseExitEvent?.Invoke();
    }
}
