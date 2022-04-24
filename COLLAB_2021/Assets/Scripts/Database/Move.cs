using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveData Base { get; set; }
    public int PP { get; set; }

    public Move(MoveData pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }

    /// <summary>
    /// Constructor to restore the save data of Move class.
    /// </summary>
    /// <param name="saveData"></param>
    public Move(MoveSaveData saveData)
    {
        Base = MoveDB.GetMoveByName(saveData.name);
        PP = saveData.pp;
    }

    public MoveSaveData GetSaveData()
    {
        var saveData = new MoveSaveData()
        {
            name = Base.Name,
            pp = Base.PP
        };

        return saveData;
    }

    //Implementation of restoring PP item.
    public void IncreasePP(int amount)
    {
        PP = Mathf.Clamp(PP + amount, 0, Base.PP);
    }
}

[System.Serializable]
public class MoveSaveData
{
    public string name;
    public int pp;
}