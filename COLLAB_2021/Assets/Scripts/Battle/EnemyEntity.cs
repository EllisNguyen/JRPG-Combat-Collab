using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [Header("Battle attributes.")]
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    GameState state;

    public string Name { get => name; }

    public Sprite Sprite { get => sprite; }

    public void InitBattle()
    {
        GameController.Instance.StartBattle(this);
    }
}
