///Author: Phap Nguyen.
///Description: Speed progressor.
///Day created: 22/02/2022
///Last edited: 05/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SpeedProgressor : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image portrait;

    public Slider Slider
    {
        get { return slider; }
        set { slider = value; }
    }

    BattleSystem battleSystem;

    /// <summary>
    /// Find the game object contains BattleSystem class.
    /// </summary>
    void OnEnable()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    public void SetProgressorData(Character character)
    {
        portrait.sprite = character.Base.battleIcon;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    /// <param name="activeUnit"></param>
    /// <returns></returns>
    public IEnumerator SpeedProgress(Character character, BattlePawn activeUnit)
    {
        int characterSpeed = character.Base.speed;
        print(character.Base.charName + " " + characterSpeed);

        while(slider.value < 1 && battleSystem.State != BattleState.Busy)
        {
            battleSystem.State = BattleState.Waiting;
            slider.value += (characterSpeed * battleSystem.SpeedProgressorMultiplier) * 0.0015f;
            yield return null;
            //await Task.Yield();
        }

        if(slider.value >= 1)
        {
            if(battleSystem.State != BattleState.Busy) battleSystem.State = BattleState.Busy;
            battleSystem.ActiveCharacter.sprite = character.Base.battleIcon;
            battleSystem.ActiveUnit = activeUnit;

            if (activeUnit.IsPlayerUnit == battleSystem.ActiveUnit.IsPlayerUnit) battleSystem.action = BattleAction.Move;
            //else StartCoroutine(battleSystem.RunTurnsEnemy(BattleAction.Wait));
        }
    }

}
