using UnityEngine;
// In EnemyStats.cs, add:


public class EnemyStats : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Movement speed of the enemy")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("Starting Health of the enemy")]
    [SerializeField] private int maxHealth = 100;
    [Tooltip("Damage the enemy does when attacking")]
    [SerializeField] private int damage = 10;
    [Tooltip("Cooldown time in seconds between attacks")]
    [SerializeField] private int attackCooldown = 1;
    [Tooltip("Delay in seconds before the enemy attacks once the player is within range")]
    [SerializeField] private int attackDelay = 1;

    public int CurrentHealth { get; private set; }
    public int Damage => damage;
    public float MoveSpeed => moveSpeed;
    public int AttackCooldown => attackCooldown;
    public int AttackDelay => attackDelay;

    public event System.Action OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    /// <summary>
    /// Reduces health by the given amount and handles death if health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return; // Already dead

        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Heals the enemy by the given amount, up to maxHealth.
    /// </summary>
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
    }

    /// <summary>
    /// Handles enemy death logic.
    /// </summary>
    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
