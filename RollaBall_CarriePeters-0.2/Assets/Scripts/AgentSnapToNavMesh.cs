using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentSnapToNavMesh : MonoBehaviour
{
    public float sampleRadius = 2f;

    void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
        var rb = GetComponent<Rigidbody>();

        Debug.Log($"{name}: isOnNavMesh={agent.isOnNavMesh}, baseOffset={agent.baseOffset}, radius={agent.radius}");

        if (rb != null)
        {
            rb.isKinematic = true; // ensure physics doesn't fight the agent
            Debug.Log($"{name}: set rb.isKinematic = true");
        }

        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, sampleRadius, NavMesh.AllAreas))
            {
                Debug.Log($"{name}: Warping to NavMesh at {hit.position}");
                agent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning($"{name}: No NavMesh within {sampleRadius} units. Move this object to a baked NavMesh area or increase sampleRadius and rebake NavMesh if needed.");
            }
        }
    }
}
