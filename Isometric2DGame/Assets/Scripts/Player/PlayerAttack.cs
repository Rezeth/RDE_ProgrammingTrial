using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the player's melee attack logic, including input, cooldown, and damaging enemies.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    // Reference to the player's stats (damage, attack range, cooldown, etc.)
    private PlayerStats playerStats;
    // Reference to the input actions for handling player input
    private InputSystem_Actions inputActions;
    // Tracks the last time the player performed a melee attack
    private float lastAttackTime = -Mathf.Infinity;

    /// <summary>
    /// Initializes references to PlayerStats and input actions.
    /// </summary>
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        inputActions = new InputSystem_Actions();
    }

    /// <summary>
    /// Enables the input system and subscribes to the melee attack action.
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MeleeAttack.performed += OnMeleeAttack;
    }

    /// <summary>
    /// Unsubscribes from the melee attack action and disables the input system.
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.MeleeAttack.performed -= OnMeleeAttack;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// Called when the melee attack input is performed.
    /// Checks cooldown and triggers the attack if allowed.
    /// </summary>
    /// <param name="context">Input action context.</param>
    private void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (Time.time < lastAttackTime + playerStats.MAttackCooldown)
            return;

        lastAttackTime = Time.time;
        PerformMeleeAttack();
    }

    /// <summary>
    /// Performs the melee attack by detecting enemies in range and applying damage.
    /// </summary>
    private void PerformMeleeAttack()
    {
        // Detect all colliders within melee attack range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, playerStats.MAttackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(playerStats.MDamage);
                    Debug.Log($"Attacked {hit.name} for {playerStats.MDamage} damage.");
                }
            }
        }
    }

    /// <summary>
    /// Visualizes the player's melee attack range in the Unity editor when selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (playerStats == null) playerStats = GetComponent<PlayerStats>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerStats != null ? playerStats.MAttackRange : 1.5f);
    }
}
