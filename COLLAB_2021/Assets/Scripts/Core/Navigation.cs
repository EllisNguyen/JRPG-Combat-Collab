///Author: Phap Nguyen.
///Description: Universal navigation for NPC and Enemy.
///Day created: 17/05/2022
///Last edited: 17/05/2022 - Phap Nguyen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public enum NavigationType { Static, Random, Waypoints}

[RequireComponent(typeof(NavMeshAgent))]
public class Navigation : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] NavigationType navigationType = NavigationType.Static;
    [SerializeField] NavMeshAgent agent;

    [ShowIf("navigationType", NavigationType.Random)][BoxGroup("RANDOM")]
    [SerializeField] float wanderRadius;
    [ShowIf("navigationType", NavigationType.Random)][BoxGroup("RANDOM")]
    [MinMaxSlider(1, 10, true)] public Vector2 wanderTimer = new Vector2(3, 5);

    [ShowIf("navigationType", NavigationType.Waypoints)][BoxGroup("WAYPOINTS")]
    [SerializeField] List<Transform> waypoints;
    [ShowIf("navigationType", NavigationType.Waypoints)][BoxGroup("WAYPOINTS")]
    [SerializeField] bool selectRandom = false;
    [ShowIf("navigationType", NavigationType.Waypoints)][BoxGroup("WAYPOINTS")]
    [MinMaxSlider(1, 10, true)] public Vector2 waypointTimer = new Vector2(3, 5);
    [ShowIf("navigationType", NavigationType.Waypoints)][BoxGroup("WAYPOINTS")]
    int waypointIndex;
    Vector3 target;

    private float timer;
    #endregion VARIABLES

    #region GETTER SETTER
    public NavigationType NavigationType => navigationType;
    public NavMeshAgent Agent => agent;

    //RANDOM WANDER
    public float WanderRadius => wanderRadius;
    public float MaxWanderTimer => wanderTimer.x;
    public float MinWanderTimer => wanderTimer.y;

    //PATROL
    public List<Transform> Waypoints => waypoints;
    public bool SelectRandom => selectRandom;
    public float MaxWaypointTimer => waypointTimer.x;
    public float MinWaypointTimer => waypointTimer.y;

    #endregion GETTER SETTER

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Agent.enabled = true;

        switch (navigationType)
        {
            case NavigationType.Static:
                break;
            case NavigationType.Random:
                timer = 0;
                break;
            case NavigationType.Waypoints:
                timer = 0;
                UpdateDestination();
                print(target);
                break;
            default:
                break;
        }
    }

    void OnDisable()
    {
        Agent.enabled = false;
    }

    void OnEnable()
    {
        Agent.enabled = true;
    }

    // Update is called once per frame
    protected void HandleUpdate()
    {
        switch (navigationType)
        {
            case NavigationType.Static:
                break;
            case NavigationType.Random:
                timer += Time.deltaTime;
                float rTimer = Random.Range(wanderTimer.x, wanderTimer.y);

                if (timer >= rTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
                break;
            case NavigationType.Waypoints:
                print($"{gameObject.name} test");
                if (Vector3.Distance(transform.position, target) < 1)
                {
                    target = waypoints[waypointIndex].position;
                    timer += Time.deltaTime;
                    float wpTimer = Random.Range(waypointTimer.x, waypointTimer.y);

                    if (timer >= wpTimer)
                    {
                        GetWaypoint();
                        UpdateDestination();
                        timer = 0;
                    }
                }
                else
                    UpdateDestination();
                break;
            default:
                break;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void GetWaypoint()
    {
        if (selectRandom)
        {
            waypointIndex = Random.Range(0, waypoints.Count);
        }
        else
        {
            if(waypointIndex == waypoints.Count)
                waypointIndex = 0;
            else
                waypointIndex++;
        }
        UpdateDestination();
    }

    void UpdateDestination()
    {
        agent.SetDestination(target);
        print(target);
    }

    private void OnDrawGizmos()
    {
        Vector3 gizmosPos = new Vector3((float)transform.position.x, (float)transform.position.y);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
