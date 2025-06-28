using UnityEngine;

/// <summary>
/// Handles the enemy's attack behavior when close enough to the player.
/// </summary>
public class EnemyAttackState : IEnemyState
{
    private readonly EnemyAI enemyAI; // Reference to the main AI controller

    /// <summary>
    /// Initializes the attack state.
    /// </summary>
    public EnemyAttackState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    /// <summary>
    /// Called when entering the attack state.
    /// </summary>
    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Attack");
    }

    /// <summary>
    /// Simulates attacking the player by logging a message each frame.
    /// </summary>
    public void Update()
    {
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
