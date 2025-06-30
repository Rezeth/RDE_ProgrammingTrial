using UnityEngine;

/// <summary>
/// Handles the enemy's idle behavior when not chasing, attacking, or patrolling.
/// </summary>
public class EnemyIdleState : IEnemyState
{
    private EnemyAI enemyAI;
    private float duration;
    private float timer; // Timer to track idle duration
    private readonly System.Action onIdleComplete; // Callback for when idle finishes

    /// <summary>
    /// Initializes the idle state.
    /// </summary>
    /// <param name="ai">Reference to the EnemyAI.</param>
    /// <param name="duration">How long to idle (seconds).</param>
    /// <param name="onIdleComplete">Callback to invoke when idle is done.</param>
    public EnemyIdleState(EnemyAI ai, float duration, System.Action onIdleComplete = null)
    {
        this.enemyAI = ai;
        this.duration = duration;
        this.timer = 0f;
        this.onIdleComplete = onIdleComplete;
    }

    /// <summary>
    /// Called when entering the idle state.
    /// </summary>
    public void Enter()
    {
        // Reset the timer and log the state entry
        timer = 0f;
        AIStateLoggingManager.LogStateEnter("Idle");
    }

    /// <summary>
    /// Called every frame while in the idle state.
    /// </summary>
    public void Update()
    {
        AIStateLoggingManager.Log("Enemy is idling...");

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            // Notify the state machine that idle is complete
            onIdleComplete?.Invoke();
        }
    }

    /// <summary>
    /// Called when exiting the idle state.
    /// </summary>
    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Idle");
    }
}
