///Author: Phap Nguyen.
///Description: ScriptableObject for monsters.
///Day created: 25/04/2022
///Last edited: 17/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Enemy Stat", menuName = "Character Stat/New Enemy")]
public class EnemyBase : CharacterBaseStats
{
    [FoldoutGroup("Battle rewards")][SerializeField] int money;
    [InfoBox("Sum of drop chance should always be 100.", InfoMessageType.Info)]
    [FoldoutGroup("Battle rewards")][SerializeField] List<BattleDrop> drops;

    public int ChanceSum()
    {
        int sum = 0;
        foreach (var item in drops)
        {
            sum += item.Chance;
        }
        return sum;
    }
}

[System.Serializable]
public class BattleDrop
{
    [SerializeField] ItemBase item;
    [MinMaxSlider(1, 10, true)]
    [SerializeField] Vector2Int count = new Vector2Int(1, 2);
    [SerializeField] int chance;

    public ItemBase Item => item;

    public int MinDrop => count.x;
    public int MaxDrop => count.y;

    public int Chance => chance;


}