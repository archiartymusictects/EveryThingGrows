using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class TugOfWarHealth : MonoBehaviour
{
    public Slider TugOfWarHealthBar; // Assign in Inspector
    public float currentTugOfWarHealth = 50; // Start at midpoint (50/100)
    public float smoothSpeed = 5f;         // How fast the bar animates
    void Start()
    {
        TugOfWarHealthBar.value = currentTugOfWarHealth; // Initialize the slider
    }
    void Update()
    {     // health changing test
        if (Input.GetKeyDown(KeyCode.X))
            PlayerDamaged(10); // Press X: Player loses 10 HP (bar moves left)

        if (Input.GetKeyDown(KeyCode.C))
            EnemyDamaged(10); // Press C: Enemy loses 10 HP (bar moves right)

      /*  if (currentTugOfWarHealth <= 0)
            Debug.Log("Player loses!"); // Enemy wins

        if (currentTugOfWarHealth >= 100)
            Debug.Log("Player wins!"); // Enemy loses */
        CheckGameOver();
        TugOfWarHealthBar.value = Mathf.Lerp(TugOfWarHealthBar.value, currentTugOfWarHealth, smoothSpeed * Time.deltaTime);

    }

    // Call this when PLAYER takes damage (moves left)
    public void PlayerDamaged(float damage)
    {
        currentTugOfWarHealth = Mathf.Max(0, currentTugOfWarHealth - damage);
        /*TugOfWarHealthBar.value = currentTugOfWarHealth;*/ // This line is now in Update() for smooth animation
    }

    // Call this when ENEMY takes damage (moves right)
    public void EnemyDamaged(float damage)
    {
        currentTugOfWarHealth = Mathf.Min(100, currentTugOfWarHealth + damage);
        /*TugOfWarHealthBar.value = currentTugOfWarHealth;*/ // This line is now in Update() for smooth animation
    }

    void CheckGameOver()
    {
        if (currentTugOfWarHealth <= 0)
        {
            Debug.Log("Player loses!"); // Enemy wins
            // Add game over logic here
        }
        else if (currentTugOfWarHealth >= 100)
        {
            Debug.Log("Player wins!"); // Enemy loses
            // Add game over logic here
        }
    }

}