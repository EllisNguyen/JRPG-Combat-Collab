using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector;
#endif

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(CharacterParty))]
public class EnemyEntity : MonoBehaviour
{
    [Header("Enemy attributes")]
    [SerializeField] EnemyBase _base;
    [ShowIf("_base")][ReadOnly][PreviewField][SerializeField] Sprite _sprite;
    [SerializeField] EnemyMovement movement;

    [BoxGroup("Detection")] [SerializeField] SphereCollider detectionCollider;
    [BoxGroup("Detection")] [Range(2,5)][SerializeField] float detectionRadius;

    [BoxGroup("Trigger")] [SerializeField] SphereCollider triggerCollider;
    [BoxGroup("Trigger")] [Range(0.6f, 0.9f)][SerializeField] float triggerRadius;

    [BoxGroup("Overworld")][SerializeField] AlterAnim animator;
    [BoxGroup("Overworld")][SerializeField] SpriteRenderer graphic;

    [Header("Battle attributes.")]
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    public CharacterParty enemyParty;
    public int enemyLevel;

    GameState state;

    public string Name { get => name; }

    public Sprite Sprite { get => sprite; }

#if UNITY_EDITOR
    void OnValidate()
    {
        enemyParty = gameObject.GetComponent<CharacterParty>();
        enemyParty.Characters.

        detectionCollider.radius = detectionRadius;
        triggerCollider.radius = triggerRadius;
        if (movement == null) movement = GetComponent<EnemyMovement>();

        if (_base != null)
        {
            _sprite = _base.overworldAnim[0];
            graphic.sprite = _sprite;

            if(animator.WalkLeftSprites != null && animator.WalkRightSprites != null)
            {
                animator.WalkLeftSprites.Clear();
                animator.WalkRightSprites.Clear();
            }

            animator.WalkLeftSprites = _base.overworldAnim;
            animator.WalkRightSprites = _base.overworldAnim;
        }
    }
#endif

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.FreeRoam) movement.HandleUpdate();
    }

    public void InitBattle()
    {
        GameController.Instance.StartBattle(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //TODO: Init battle.
            InitBattle();
            print("battle start");
        }
    }
}
