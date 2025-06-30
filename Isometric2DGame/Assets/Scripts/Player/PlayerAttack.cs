using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the player's melee and ranged attack logic, including input, cooldowns, projectile firing, and damaging enemies.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Tooltip("Prefab for the player's ranged projectile attack.")]
    [SerializeField] private GameObject projectilePrefab;

    // Reference to the player's stats (damage, attack range, cooldowns, etc.)
    private PlayerStats playerStats;
    // Handles input actions for the player (melee and ranged attacks)
    private InputSystem_Actions inputActions;
    // Tracks the last time the player performed a melee attack (for cooldown)
    private float lastAttackTime = -Mathf.Infinity;
    // Tracks the last time the player fired a projectile (for cooldown)
    private float lastFireTime = -Mathf.Infinity;
    // Reference to the main camera, used for aiming ranged attacks
    private Camera mainCamera;

    /// <summary>
    /// Initializes references to PlayerStats, input actions, and the main camera.
    /// </summary>
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        inputActions = new InputSystem_Actions();
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Enables the input system and subscribes to melee and ranged attack actions.
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MeleeAttack.performed += OnMeleeAttack;
        inputActions.Player.RangedAttack.performed += OnRangedAttack;
    }

    /// <summary>
    /// Unsubscribes from attack actions and disables the input system.
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.MeleeAttack.performed -= OnMeleeAttack;
        inputActions.Player.RangedAttack.performed -= OnRangedAttack;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// Called when the ranged attack input is performed.
    /// Attempts to fire a projectile if the cooldown has elapsed.
    /// </summary>
    /// <param name="context">Input action context.</param>
    private void OnRangedAttack(InputAction.CallbackContext context)
    {
        TryFireProjectile();
    }

    /// <summary>
    /// Attempts to fire a projectile towards the mouse position if the ranged attack cooldown has elapsed.
    /// </summary>
    private void TryFireProjectile()
    {
        Debug.Log("Attempting to fire projectile...");
        if (Time.time - lastFireTime < playerStats.RAttackCooldown)
            return;

        lastFireTime = Time.time;

        // Get mouse position in world space
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        // Calculate direction from player to mouse
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        // Instantiate and initialize the projectile
        var projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var projectile = projectileObj.GetComponent<PlayerProjectile>();
        projectile.Initialize(
            direction,
            playerStats.RProjectileSpeed,
            playerStats.RAttackRange,
            playerStats.RDamage
        );
    }

    /// <summary>
    /// Called when the melee attack input is performed.
    /// Checks cooldown and triggers the melee attack if allowed.
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
    /// Performs the melee attack by detecting enemies in range and applying damage to them.
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
        // Optionally: play attack animation or sound here
    }

    /// <summary>
    /// Visualizes the player's melee attack range in the Unity editor when the player is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (playerStats == null) playerStats = GetComponent<PlayerStats>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerStats != null ? playerStats.MAttackRange : 1.5f);
    }
}
