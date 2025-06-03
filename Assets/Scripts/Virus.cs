
using UnityEngine;

public abstract class Virus : Cell
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.EnemyTakeDamage(10);
            Destroy(gameObject);
        }
    }
}
