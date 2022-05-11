///Author: Phap Nguyen.
///Description: Spawnpoint for battle system.
///Day created: 10/05/2022
///Last edited: 10/05/2022 - Phap Nguyen.

using UnityEngine;

public class BattleSpawnPoint : MonoBehaviour
{

    [SerializeField] BattlePawn activeUnit;
    [SerializeField] bool hasActiveUnit = false;

    public BattlePawn ActiveUnit
    {
        get { return activeUnit; }
        set { activeUnit = value; }
    }

    public bool HasActiveUnit
    {
        get { return hasActiveUnit; }
        set { hasActiveUnit = value; }
    }

    public void SetActiveSpawnpoint(BattlePawn activeUnit)
    {
        this.activeUnit = activeUnit;
        hasActiveUnit = true;
    }

    public void ClearActiveSpawnpoint()
    {
        this.activeUnit = null;
        hasActiveUnit = false;
    }
}
