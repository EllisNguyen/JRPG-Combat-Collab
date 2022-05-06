using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
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
    public CharacterParty party;
    public int enemyLevel;
    public bool isMoving;

    GameState state;

    public string Name { get => name; }

    public Sprite Sprite { get => sprite; }

#if UNITY_EDITOR
    void OnValidate()
    {
        party = gameObject.GetComponent<CharacterParty>();

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
        //party.AddCharacter();
    }
#endif

    void Update()
    {
        if (GameManager.Instance.gameState != GameState.FreeRoam)
        {
            movement.enemy.enabled = false;
            return;
        }
        else
        {
            movement.enemy.enabled = true;
        }

        if (movement.GetMovementDirection().x > 0)
        {
            animator.MoveX = movement.GetMovementDirection().x;
            animator.FlipSprite(false);
        }
        else
        {
            animator.MoveX = movement.GetMovementDirection().x;
            animator.FlipSprite(true);
        }

        movement.HandleUpdate();

        if (movement.enemy.velocity.sqrMagnitude != 0)
        {
            isMoving = true;
        }
        else
            isMoving = false;

        animator.IsMoving = isMoving;
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
