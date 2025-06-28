using UnityEngine;

/// <summary>
/// Handles the enemy's patrol behavior, moving between two patrol points.
/// </summary>
public class EnemyPatrolState : IEnemyState
{
    private readonly EnemyAI enemyAI;                // Reference to the main AI controller
    private readonly Vector2[] patrolPoints;         // The patrol points to move between
    private int currentTargetIndex = 0;              // Index of the current patrol target
    private readonly float moveSpeed;                // Movement speed while patrolling
    private readonly float pointReachedThreshold = 0.1f; // Distance threshold to consider a point reached

    /// <summary>
    /// Initializes the patrol state with patrol points and movement speed.
    /// </summary>
    public EnemyPatrolState(EnemyAI enemyAI, Vector2[] patrolPoints, float moveSpeed)
    {
        this.enemyAI = enemyAI;
        this.patrolPoints = patrolPoints;
        this.moveSpeed = moveSpeed;
    }

    /// <summary>
    /// Called when entering the patrol state.
    /// </summary>
    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Patrol");
    }

    /// <summary>
    /// Moves the enemy toward the current patrol point and switches to the next when reached.
    /// </summary>
    public void Update()
    {
        Vector2 currentPosition = enemyAI.transform.position;
        Vector2 target = patrolPoints[currentTargetIndex];
        Vector2 direction = (target - currentPosition).normalized;
        enemyAI.transform.position = Vector2.MoveTowards(currentPosition, target, moveSpeed * Time.deltaTime);

        AIStateLoggingManager.Log("Enemy is patrolling...");

        // Switch to the next patrol point if close enough to the current one
        if (Vector2.Distance(currentPosition, target) < pointReachedThreshold)
        {
            currentTargetIndex = (currentTargetIndex + 1) % patrolPoints.Length;
        }
    }

    /// <summary>
    /// Called when exiting the patrol state.
    /// </summary>
    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Patrol");
    }
}
