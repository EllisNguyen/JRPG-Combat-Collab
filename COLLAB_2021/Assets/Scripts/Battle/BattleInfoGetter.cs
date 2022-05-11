using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfoGetter : MonoBehaviour
{
    public BattleSystem battleSystem;

    // Start is called before the first frame update
    void Start()
    {
        var player = GameController.Instance.player;
        var enemy = GameController.Instance.enemy;

        battleSystem.player = player;
        battleSystem.enemy = enemy;

        battleSystem.playerParty = player.party;
        battleSystem.enemyParty = enemy.party;

        //battleSystem.SetupBattle(battleSystem.player.);

        battleSystem.StartBattle(battleSystem.playerParty, battleSystem.enemyParty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
