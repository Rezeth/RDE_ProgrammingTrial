using UnityEngine;

/// <summary>
/// Handles the enemy's attack behavior when close enough to the player.
/// </summary>
public class EnemyAttackState : IEnemyState
{
    private readonly EnemyAI enemyAI; // Reference to the main AI controller
    private EnemyStats enemyStats;
    private PlayerHealth playerHealth;
    private float lastAttackTime = -Mathf.Infinity;
    private float stateEnterTime;


    /// <summary>
    /// Initializes the attack state.
    /// </summary>
    public EnemyAttackState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
        enemyStats = enemyAI.GetComponent<EnemyStats>();
        var playerTransform = enemyAI.Player;
        if (playerTransform != null)
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
    }

    /// <summary>
    /// Called when entering the attack state.
    /// </summary>
    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Attack");
        stateEnterTime = Time.time;
        lastAttackTime = -Mathf.Infinity;
    }

    /// <summary>
    /// Simulates attacking the player by logging a message each frame.
    /// </summary>
    public void Update()
    {
        // Wait for an initial attack delay after entering the state
        if (Time.time < stateEnterTime + enemyStats.AttackDelay)
        {
            AIStateLoggingManager.Log("Enemy is preparing to attack...");
            return;
        }

        if (playerHealth != null && Time.time >= lastAttackTime + enemyStats.AttackCooldown)
        {
            playerHealth.TakeDamage(enemyStats.Damage);
            lastAttackTime = Time.time;
        }
        AIStateLoggingManager.Log("Enemy is attacking the player...");
    }

    /// <summary>
    /// Called when exiting the attack state.
    /// </summary>
    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Attack");
    }
}
