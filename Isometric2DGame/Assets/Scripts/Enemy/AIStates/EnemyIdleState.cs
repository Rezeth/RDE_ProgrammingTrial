using UnityEngine;

/// <summary>
/// Handles the enemy's idle behavior when not chasing, attacking, or patrolling.
/// </summary>
public class EnemyIdleState : IEnemyState
{
    private readonly EnemyAI enemyAI; // Reference to the main AI controller

    /// <summary>
    /// Initializes the idle state.
    /// </summary>
    public EnemyIdleState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    /// <summary>
    /// Called when entering the idle state.
    /// </summary>
    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Idle");
    }

    /// <summary>
    /// Called every frame while in the idle state.
    /// </summary>
    public void Update()
    {
        AIStateLoggingManager.Log("Enemy is idling...");
    }

    /// <summary>
    /// Called when exiting the idle state.
    /// </summary>
    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Idle");
    }
}
