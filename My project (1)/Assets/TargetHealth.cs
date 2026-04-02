using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int points = 1;

    public int currentHealth;

    private GameManager gameManager;

    public GameManager GameManager { get { return gameManager;  } set { gameManager = value;  } }

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void DisableTarget()
    {
        Debug.Log("Pew");
        if(gameManager != null )
        {
            gameManager.AddScore(points);
        }
        gameObject.SetActive(false);
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        {
            if (currentHealth <= 0)
            {
                DisableTarget();
            }
        }
    }
}