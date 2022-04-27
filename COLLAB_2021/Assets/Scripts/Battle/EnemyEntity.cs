using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(EnemyMovement))]
public class EnemyEntity : MonoBehaviour
{
    [Header("Enemy attributes")]
    [SerializeField] EnemyMovement movement;
    [SerializeField] SphereCollider detectionCollider;
    [Range(2,5)][SerializeField] float detectionRadius;

    [Header("Battle attributes.")]
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    GameState state;

    public string Name { get => name; }

    public Sprite Sprite { get => sprite; }

#if UNITY_EDITOR
    void OnValidate()
    {
        detectionCollider.radius = detectionRadius;

        if (movement == null) movement = GetComponent<EnemyMovement>();
    }
#endif

    public void InitBattle()
    {
        GameController.Instance.StartBattle(this);
    }
}
