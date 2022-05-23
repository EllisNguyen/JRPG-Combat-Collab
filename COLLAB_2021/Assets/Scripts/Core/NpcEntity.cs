using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcEntity : Navigation
{

    [SerializeField] float movementSpeed = 2.5f;
    [BoxGroup("Overworld")][SerializeField] AlterAnim animator;
    [BoxGroup("Overworld")][SerializeField] SpriteRenderer graphic;
    public bool isMoving;

    void Start()
    {
        base.Start();
        Agent.speed = movementSpeed;
    }

    public void HandleUpdate()
    {
        base.HandleUpdate();
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameState.FreeRoam)
        {
            Agent.enabled = false;
            animator.IsMoving = false;
            return;
        }
        else
        {
            Agent.enabled = true;
        }

        animator.MoveX = GetMovementDirection().x;
        if (GetMovementDirection().x != 0)
        {
            if (GetMovementDirection().x > 0)
            {
                animator.FlipSprite(false);
            }
            else
            {
                animator.FlipSprite(true);
            }
        }

        HandleUpdate();

        if (Agent.velocity.sqrMagnitude != 0)
        {
            isMoving = true;
        }
        else
            isMoving = false;

        animator.IsMoving = isMoving;
    }

    public Vector3 GetMovementDirection()
    {
        Vector3 normalizedDirection = Agent.desiredVelocity.normalized;
        return normalizedDirection;
    }
}
