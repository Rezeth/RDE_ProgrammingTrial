using UnityEngine;
/// <summary>
/// Handles the enemy's core stats, damage, healing, and health bar updates.
/// </summary>
public class EnemyStats : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Movement speed of the enemy")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("Starting Health of the enemy")]
    [SerializeField] private int maxHealth = 100;

    [Header("Combat Stats")]
    [Tooltip("Damage the enemy does when attacking")]
    [SerializeField] private int damage = 10;
    [Tooltip("Cooldown time in seconds between attacks")]
    [SerializeField] private int attackCooldown = 1;
    [Tooltip("Delay in seconds before the enemy attacks once the player is within range")]
    [SerializeField] private int attackDelay = 1;
    [Tooltip("Attack range of the enemy")]
    [SerializeField] private float attackRange = 1.2f;

    [Header("Self-Healing")]
    [Tooltip("Amount of health the enemy heals each time")]
    [SerializeField] private int healAmount = 10;
    [Tooltip("Cooldown time in seconds between heals")]
    [SerializeField] private float healCooldown = 5f;

    // Public properties for accessing stat values
    public int CurrentHealth { get; private set; }
    public int Damage => damage;
    public float MoveSpeed => moveSpeed;
    public int AttackCooldown => attackCooldown;
    public int AttackDelay => attackDelay;
    public float AttackRange => attackRange;
    public int HealAmount => healAmount;
    public float HealCooldown => healCooldown;

    // Event triggered when the enemy dies
    public event System.Action OnDeath;

    // Reference to the floating health bar UI
    private FloatingHealthBar floatingHealthBar;

    // Tracks the last time the enemy healed
    private float lastHealTime = -Mathf.Infinity;
    // Tracks if the enemy has taken damage since last being at full health
    private bool hasTakenDamageFromFull = false;

    /// <summary>
    /// Initializes health and health bar reference.
    /// </summary>
    private void Awake()
    {
        CurrentHealth = maxHealth;
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        if (floatingHealthBar != null)
        {
            floatingHealthBar.UpdateHealthBar(CurrentHealth, maxHealth);
        }
    }

    /// <summary>
    /// Reduces health by the given amount and handles death if health reaches zero.
    /// Also checks if the heal cooldown needs to be reset.
    /// </summary>
    /// <param name="amount">Amount of damage to take.</param>
    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return; // Already dead

        // Only reset heal cooldown if this is the first damage from full health
        if (!hasTakenDamageFromFull && CurrentHealth == maxHealth)
        {
            ResetHealCooldownOnDamage();
        }

        CurrentHealth -= amount;
        UpdateHealthbar();

        if (CurrentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} has died.");
            Die();
        }
    }

    /// <summary>
    /// Heals the enemy by the given amount, up to maxHealth.
    /// Resets the damage flag if healed to full.
    /// </summary>
    /// <param name="amount">Amount to heal.</param>
    public void Heal(int amount)
    {
        int prevHealth = CurrentHealth;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);

        // If healed to full, reset the flag so next damage will reset the cooldown
        if (CurrentHealth == maxHealth && prevHealth < maxHealth)
        {
            hasTakenDamageFromFull = false;
        }
    }

    /// <summary>
    /// Attempts to heal the enemy if enough time has passed since last heal and not at full health.
    /// </summary>
    public void TryHeal()
    {
        if (CurrentHealth < maxHealth && Time.time >= lastHealTime + healCooldown)
        {
            Heal(healAmount);
            lastHealTime = Time.time;
            Debug.Log($"{gameObject.name} healed for {healAmount}. Current health: {CurrentHealth}/{maxHealth}");
            UpdateHealthbar();
        }
    }

    /// <summary>
    /// Resets the heal cooldown and marks that the enemy has taken damage from full health.
    /// </summary>
    private void ResetHealCooldownOnDamage()
    {
        lastHealTime = Time.time;
        hasTakenDamageFromFull = true;
    }


    /// <summary>
    /// Handles enemy death logic, triggers OnDeath event and destroys the GameObject.
    /// </summary>
    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    /// <summary>
    /// Updates the floating health bar UI to reflect current health.
    /// </summary>
    private void UpdateHealthbar()
    {
        if (floatingHealthBar != null)
        {
            floatingHealthBar.UpdateHealthBar(CurrentHealth, maxHealth);
            Debug.Log($"Updated health bar for {gameObject.name}: {CurrentHealth}/{maxHealth}");
        }
    }

    /// <summary>
    /// Visualizes the enemy's attack range in the Unity editor when selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
