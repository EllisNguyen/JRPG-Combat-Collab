using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[Serializable]
public class UniversalInterface : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
