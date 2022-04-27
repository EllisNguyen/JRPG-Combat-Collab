using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfoGetter : MonoBehaviour
{
    public BattleSystem battleSystem;

    // Start is called before the first frame update
    void Start()
    {
        battleSystem = GetComponent<BattleSystem>();

        battleSystem.player = GameController.Instance.player;
        battleSystem.enemy = GameController.Instance.enemy;

        //battleSystem.SetupBattle(battleSystem.player.);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
