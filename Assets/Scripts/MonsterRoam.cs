using UnityEngine;
using UnityEngine.AI;

public class MonsterRoam : MonoBehaviour
{
    public float roamRadius = 10f;
    public float visualY = 0.5f; // How high the monster appears above the NavMesh

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = visualY; // Lift monster visually above NavMesh

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position); // Snap to valid NavMesh point
            SetNewDestination();
        }
        else
        {
            Debug.LogWarning("Monster not on NavMesh! Check placement and bake settings.");
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection.y = 0f;
        Vector3 targetPosition = transform.position + randomDirection;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navHit, roamRadius, NavMesh.AllAreas))
        {
            Vector3 clampedPosition = new Vector3(navHit.position.x, navHit.position.y, navHit.position.z);
            agent.SetDestination(clampedPosition);
            Debug.Log("Monster roaming to: " + clampedPosition);
        }
        else
        {
            Debug.LogWarning("No valid NavMesh point found near: " + targetPosition);
        }
    }

    void OnDrawGizmos()
    {
        if (agent != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(agent.destination, 0.3f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}
