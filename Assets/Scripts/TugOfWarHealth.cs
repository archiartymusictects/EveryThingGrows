using UnityEngine;
using UnityEngine.UI;

public class TugOfWarHealth : MonoBehaviour
{
    public Slider TugOfWarHealthBar; // Assign in Inspector
    public float currentTugOfWarHealth = 50; // Start at midpoint (50/100)

    void Start()
    {
        TugOfWarHealthBar.value = currentTugOfWarHealth; // Initialize the slider
    }

    // Call this when PLAYER takes damage (moves left)
    public void PlayerDamaged(float damage)
    {
        currentTugOfWarHealth = Mathf.Max(0, currentTugOfWarHealth - damage);
        TugOfWarHealthBar.value = currentTugOfWarHealth;
    }

    // Call this when ENEMY takes damage (moves right)
    public void EnemyDamaged(float damage)
    {
        currentTugOfWarHealth = Mathf.Min(100, currentTugOfWarHealth + damage);
        TugOfWarHealthBar.value = currentTugOfWarHealth;
    }
    void Update()
    {     // health changing test
        if (Input.GetKeyDown(KeyCode.X))
            PlayerDamaged(10); // Press X: Player loses 10 HP (bar moves left)

        if (Input.GetKeyDown(KeyCode.C))
            EnemyDamaged(10); // Press C: Enemy loses 10 HP (bar moves right)
    }


}