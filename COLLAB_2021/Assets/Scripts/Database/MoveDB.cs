using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDB
{

    //Dictionary to store all move data using Key string and value of the moves.
    static Dictionary<string, MoveData> moves;

    public static void Init()
    {
        //Declare the dictionary.
        moves = new Dictionary<string, MoveData>();

        //Find all MoveData class in Resources directory and load them into the Dictionary.
        var moveList = Resources.LoadAll<MoveData>("");

        //Loop through all creature data.
        foreach (var move in moveList)
        {
            //Check for similar key before adding data to the dictionary.
            if (moves.ContainsKey(move.Name))
            {
                Debug.LogError($"There are multiple creature with the name {move.Name}");
                continue;
            }

            moves[move.Name] = move;
        }
    }

    public static MoveData GetMoveByName(string name)
    {
        if (!moves.ContainsKey(name))
        {
            Debug.LogError($"{name} not found in the database.");
            return null;
        }

        return moves[name];
    }

}
