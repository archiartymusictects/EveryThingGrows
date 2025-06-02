using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxEnemyHealth = 100;         // Total health
    private int currentEnemyHealth;           // Current health
    public Slider WhiteBloodCells;             // Reference to the UI Slider

    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;       // Initialize health
        if (WhiteBloodCells != null)
        {
            WhiteBloodCells.maxValue = maxEnemyHealth;  // Set slider max
            WhiteBloodCells.value = currentEnemyHealth; // Update slider
        }
    }

    public void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;         // Reduce health
        if (WhiteBloodCells != null)
            WhiteBloodCells.value = currentEnemyHealth; // Update slider

        if (currentEnemyHealth <= 0)
            Destroy(gameObject);         // Die if health ≤ 0
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            TakeDamage(10); // Press SPACE to deal 10 damage
    }
}