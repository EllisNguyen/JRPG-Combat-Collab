///Author: Phap Nguyen.
///Description: Container of the Inventory, control the spawn of inventory object and UI.
///Day created: 22/03/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;//TextMeshPro lib.

public class CharacterAboutPanel : MonoBehaviour
{
    #region Variables
    //Idk why these var existed :/
    [Header("Info")]
    [SerializeField] string characterName;
    [SerializeField] string characterLevel;
    [SerializeField] string characterDescription;

    //Character info UI displacement.
    [Header("UI stuff")]
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] TextMeshProUGUI descriptionTxt;
    [SerializeField] Image portrait;

    //Store the stats into these var.
    int hp;
    int mp;
    int atk;
    int def;
    int spatk;
    int spdef;
    int spd;

    //Progressor for the stats.
    [Header("Stats UI")]
    [SerializeField] Image hpProgressor;
    [SerializeField] Image mpProgressor;
    [SerializeField] Image atkProgressor;
    [SerializeField] Image defProgressor;
    [SerializeField] Image spAtkProgressor;
    [SerializeField] Image spDefProgressor;
    [SerializeField] Image spdProgressor;

    //Text element for the stats.
    [Header("Number")]
    [SerializeField] TextMeshProUGUI hpTxt;
    [SerializeField] TextMeshProUGUI mpTxt;
    [SerializeField] TextMeshProUGUI atkTxt;
    [SerializeField] TextMeshProUGUI defTxt;
    [SerializeField] TextMeshProUGUI spatkTxt;
    [SerializeField] TextMeshProUGUI spdefTxt;
    [SerializeField] TextMeshProUGUI spdTxt;
    #endregion Variables

    #region Functions
    /// <summary>
    /// Call upon setting up the information for selected character.
    /// </summary>
    /// <param name="selectedCharacter"></param>
    public void SetAboutUI(Character selectedCharacter)
    {
        //Store data from Character to local var(s).
        #region data number
        hp = selectedCharacter.Base.health;
        mp = selectedCharacter.Base.mana;
        atk = selectedCharacter.Base.physicalAtkDmg;
        def = selectedCharacter.Base.physicalDef;
        spatk = selectedCharacter.Base.specialAtkDmg;
        spdef = selectedCharacter.Base.specialDef;
        spd = selectedCharacter.Base.MaxSpeed;
        #endregion data number

        //Set UI text for stats.
        #region Set UI text
        hpTxt.text = hp.ToString();
        mpTxt.text = hp.ToString();
        atkTxt.text = atk.ToString();
        defTxt.text = def.ToString();
        spatkTxt.text = spatk.ToString();
        spdefTxt.text = spdef.ToString();
        spdTxt.text = spd.ToString();
        #endregion Set UI text

        //Set character's info on UI:
        //Name, desc, portrait, level.
        #region Set character's info
        //name
        nameTxt.text = selectedCharacter.Base.charName;
        descriptionTxt.text = selectedCharacter.Base.charDescription;

        //sprite
        portrait.sprite = selectedCharacter.Base.portraitSprite;

        //level
        levelTxt.text = "Lv." + selectedCharacter.Level;
        #endregion Set character's info

        //Run the SmoothFillAmount operation for all set stats.
        #region Set progressor
        SmoothFillAmount(hpProgressor, (float)hp / 200);
        SmoothFillAmount(mpProgressor, (float)mp / 200);
        SmoothFillAmount(atkProgressor, (float)atk / 200);
        SmoothFillAmount(defProgressor, (float)def / 200);
        SmoothFillAmount(spAtkProgressor, (float)spatk / 200);
        SmoothFillAmount(spDefProgressor, (float)spdef / 200);
        SmoothFillAmount(spdProgressor, (float)spd / 200);
        #endregion Set progressor
    }

    //Rerun the SmoothFillAmount operation.
    public void ResetFillammount()
    {
        SmoothFillAmount(hpProgressor, (float)hp / 200);
        SmoothFillAmount(mpProgressor, (float)mp / 200);
        SmoothFillAmount(atkProgressor, (float)atk / 200);
        SmoothFillAmount(defProgressor, (float)def / 200);
        SmoothFillAmount(spAtkProgressor, (float)spatk / 200);
        SmoothFillAmount(spDefProgressor, (float)spdef / 200);
        SmoothFillAmount(spdProgressor, (float)spd / 200);
    }

    /// <summary>
    /// Async operation to smoothly fill the progressor from value of 0 to newVal.
    /// </summary>
    /// <param name="progressBar">Require the Image component with ImageType of Filled to get access to fillAmount function.</param>
    /// <param name="newVal">Value that the fillAmount of the progress bar will fill up to.</param>
    public async void SmoothFillAmount(Image progressBar, float newVal)
    {
        progressBar.fillAmount = 0;

        while (progressBar.fillAmount < newVal)
        {
            progressBar.fillAmount += 0.5f * Time.deltaTime;
            await Task.Yield();
        }
        progressBar.fillAmount = newVal;
    }
    #endregion Functions
}
