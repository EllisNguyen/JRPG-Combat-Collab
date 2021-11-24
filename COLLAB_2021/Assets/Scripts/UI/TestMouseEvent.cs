///Author: Phab Nguyen
///Description: Test mouse over and interaction func.
///Day created: 05/11/2021
///Last edited: 15/11/2021 - Phab Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TestMouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float rotateAngle;
    [SerializeField] float rotateSpeed;
    //[SerializeField] AudioClip hoverAudio;
    GameObject button;

    void Awake()
    {
        button = this.gameObject;
        button.transform.localRotation = Quaternion.identity;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.transform.DORotate(new Vector3(0, 0, rotateAngle), rotateSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.transform.DORotate(new Vector3(0, 0, 0), rotateSpeed);
    }
}
