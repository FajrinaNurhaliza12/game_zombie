using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Start()
    {
        currentHealth = maxHealth;

        Debug.Log("HP Player: " + currentHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(
            currentHealth - amount,
            0,
            maxHealth
        );

        Debug.Log(
            "Player terkena damage! HP sekarang: " + currentHealth
        );

        if (currentHealth <= 0)
        {
            Debug.Log("PLAYER MATI!");
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(
            currentHealth + amount,
            0,
            maxHealth
        );

        Debug.Log(
            "HP bertambah! HP sekarang: " + currentHealth
        );
    }
}