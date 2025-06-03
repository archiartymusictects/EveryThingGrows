using UnityEngine;
using UnityEngine.AI;

public class UnitTest : MonoBehaviour
{
    NavMeshAgent agent;
    Transform targetDestination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        targetDestination = FindFirstObjectByType<EnemyBaseTest>().transform;
    }

    void Start()
    {
        agent.SetDestination(targetDestination.position);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
