using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitTest : MonoBehaviour
{
    NavMeshAgent agent;
    Transform targetDestination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (gameObject.CompareTag("PlayerUnit"))
        {
            targetDestination = FindFirstObjectByType<EnemyBaseTest>().transform;
            agent.SetDestination(targetDestination.position);
        }
        else
        {
            targetDestination = FindFirstObjectByType<PlayerBase>().transform;
            agent.SetDestination(targetDestination.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.EnemyTakeDamage(20);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerTakeDamage(20);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
       
        
        GameManager.instance.GainExp();
        
        Destroy(gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        agent.SetDestination(targetDestination.position);
    }
}
