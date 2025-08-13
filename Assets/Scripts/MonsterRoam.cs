using UnityEngine;
using UnityEngine.AI;

public class MonsterRoam : MonoBehaviour
{
    public float roamRadius = 10f; // How far from the monster can it roam
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewDestination();
    }

    void Update()
    {
        // Check if agent reached the destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position; // Center around monster's position

        NavMeshHit navHit;
        // Find a valid position on the NavMesh near the random point
        if (NavMesh.SamplePosition(randomDirection, out navHit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }
}
