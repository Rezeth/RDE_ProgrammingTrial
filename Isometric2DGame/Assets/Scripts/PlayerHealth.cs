using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the player's health logic, including taking damage, healing, and updating the health bar UI.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Tooltip("UI Image component that visually represents the player's health bar fill.")]
    [SerializeField] private Image healthBarFill;

    // Reference to the player's stats (max health, etc.)
    private PlayerStats playerStats;
    // Tracks the player's current health value
    private int currentHealth;

    /// <summary>
    /// Initializes the player's health and health bar at the start of the game.
    /// </summary>
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        currentHealth = playerStats.MaxHealth;
        UpdateHealthBar();
    }

    /// <summary>
    /// Reduces the player's health by the specified amount and updates the health bar.
    /// </summary>
    /// <param name="amount">Amount of damage to take.</param>
    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        UpdateHealthBar();
    }

    /// <summary>
    /// Increases the player's health by the specified amount, up to max health, and updates the health bar.
    /// </summary>
    /// <param name="amount">Amount to heal.</param>
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, playerStats.MaxHealth);
        UpdateHealthBar();
    }

    /// <summary>
    /// Updates the health bar UI to reflect the player's current health.
    /// </summary>
    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / playerStats.MaxHealth;
    }
}
