using UnityEngine;

/// <summary>
/// Handles the enemy's chase behavior, moving toward the player when in range.
/// </summary>
public class EnemyChaseState : IEnemyState
{
    private readonly EnemyAI enemyAI;    // Reference to the main AI controller
    private readonly Transform player;   // Reference to the player
    private readonly float moveSpeed;    // Movement speed while chasing

    /// <summary>
    /// Initializes the chase state with the player and movement speed.
    /// </summary>
    public EnemyChaseState(EnemyAI enemyAI, Transform player, float moveSpeed)
    {
        this.enemyAI = enemyAI;
        this.player = player;
        this.moveSpeed = moveSpeed;
    }

    /// <summary>
    /// Called when entering the chase state.
    /// </summary>
    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Chase");
    }

    /// <summary>
    /// Moves the enemy toward the player each frame.
    /// </summary>
    public void Update()
    {
        if (player == null) return;

        // Move towards the player
        Vector2 direction = (player.position - enemyAI.transform.position).normalized;
        enemyAI.transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        AIStateLoggingManager.Log("Enemy is chasing the player...");
    }

    /// <summary>
    /// Called when exiting the chase state.
    /// </summary>
    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Chase");
    }
}
