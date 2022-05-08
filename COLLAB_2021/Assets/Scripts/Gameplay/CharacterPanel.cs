///Author: Phap Nguyen.
///Description: Class that control the panel showing brief character info on opening the menu.
///Day created: 20/01/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CharacterPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Character _character;

    [Header("Info")]
    [SerializeField] string characterName;
    [SerializeField] string characterLevel;

    [Header("UI stuff")]
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] Image portrait;
    [SerializeField] RectTransform holder;
    Vector3 originPos;
    [SerializeField] Vector3 moveToPos;

    [Header("Stat")]
    [SerializeField] HpBar hpBar;//Ref to HP bar class.
    [SerializeField] MpBar mpBar;
    [SerializeField] Image expBar;

    CharacterAboutPanel aboutPanel;
    [SerializeField] Image appearImg;

    public UnityEvent onClick;

    public void ResetAppearAnim()
    {
        appearImg.fillAmount = 1;
    }

    void Start()
    {
        aboutPanel = GameManager.Instance.characterAbout;

        originPos = holder.anchoredPosition3D;      
    }

    public void Init(Character character)
    {
        _character = character;
        characterName = _character.Base.charName;
        characterLevel = "Lv. " + _character.Level;
        UpdateData();
        SetUi();

        //Subcribe this and run UpdateData().
        _character.OnHPChanged += UpdateData;
    }

    public void SetUi()
    {
        #region Set character's info
        //name
        nameTxt.text = characterName;

        //sprite
        portrait.sprite = _character.Base.portraitSprite;

        //level
        levelTxt.text = "Lv." + _character.Level;
        #endregion Set character's info
    }

    public void GetAboutInfo()
    {
        aboutPanel.gameObject.SetActive(true);
        aboutPanel.SetAboutUI(_character);
    }

    void UpdateData()
    {
        //Set current health on health bar.
        hpBar.SetHP((float)_character.HP / _character.MaxHP, _character);
        mpBar.SetMP((float)_character.MP / _character.MaxMP, _character);

        expBar.fillAmount = _character.Exp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        holder.DOAnchorPos3D(moveToPos, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        holder.DOAnchorPos3D(originPos, 0.1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        holder.DOAnchorPos3D(originPos, 0.1f);
        onClick?.Invoke();
    }

    public IEnumerator EnableAnim()
    {
        gameObject.SetActive(true);
        while (appearImg.fillAmount > 0)
        {
            appearImg.fillAmount -= 7f * Time.deltaTime;

            yield return null;
        }
    }
}
