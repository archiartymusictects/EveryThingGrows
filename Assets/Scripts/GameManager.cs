using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField ]private int tugOfWarHealth;

    //Player variables
    [SerializeField] private float currency;
    private float currencyGrowthRate = 0.1f;
    [SerializeField] private float exp;
    [SerializeField] private int level;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject growEffect;
    [SerializeField] private TugOfWarHealth health;


    [SerializeField] private TextMeshProUGUI gameWinText; //could switch these to a gameobject prefab to do a whole game win/lose screen
    [SerializeField] private TextMeshProUGUI gameOverText;
    


    private void Start()
    {
        instance = this;

        tugOfWarHealth = 50; 
        exp = 0; 
        level = 1;
        currency = 100;

        //subscribe to events here?
     
    }

    private void Update()
    {
        

        

    }

    private void FixedUpdate()
    {
        currency += currencyGrowthRate; //player currency generates slowly over time, maybe adjust rate when virus grows? 
    }

    public float GetCurrency()
    {
        return currency;
    }
    public void ChangeCurrency(int change)
    {
        currency += change;
    }    

    void GameWin()
    {
        gameWinText.enabled = true;
    }

    void GameOver()
    {
        gameOverText.enabled = true;
    }

    private void Grow()
    {
        level++;

        // logic for virus growth here, keep it simple and change scale?
        player.transform.localScale = new Vector3(level, level, level);
        growEffect.SetActive(true);
    }

    public void GainExp()
    {
        exp += 10;
        if (exp >= 100)
        {
            Grow();
            exp = 0;
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        tugOfWarHealth -= damage;
        health.PlayerDamaged(damage);

        if (tugOfWarHealth <= 0)
        {
            //GameOver();
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        tugOfWarHealth += damage;
        health.EnemyDamaged(damage);
        if (tugOfWarHealth >= 100)
        {
           // GameWin();
        }

    }
}
