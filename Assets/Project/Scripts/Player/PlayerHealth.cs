using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log(
            "Player menerima " + damage +
            " damage. HP: " + currentHealth + "/" + maxHealth
        );

        if (currentHealth <= 0)
        {
            Debug.Log("Player defeated!");
        }
    }
}