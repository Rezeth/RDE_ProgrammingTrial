using UnityEngine;

/// <summary>
/// Controls the enemy's AI state machine, handling transitions between different states.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Tooltip("Distance at which the enemy starts chasing the player")]
    [SerializeField] private float chaseRadius = 5f;
    [Tooltip("Distance at which the enemy starts attacking the player")]
    [SerializeField] private float attackRange = 1.2f;
    [Tooltip("Movement speed of the enemy")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("Patrol points for the enemy to move between. If empty, random points will be generated around spawn.")]
    [SerializeField] private Vector2[] patrolPoints;
    [Tooltip("Number of random patrol points to generate if none are set in the inspector.")]
    [SerializeField] private int randomPatrolPointCount = 2;
    [Tooltip("Radius around spawn for generating patrol points")]
    [SerializeField] private float patrolRadius = 3f;
    [Tooltip("Minimum distance between patrol points")]
    [SerializeField] private float minPatrolPointDistance = 1.5f;
    [Tooltip("Reference to the player transform, defaults to an object the the 'Player' tag")]
    [SerializeField] private Transform player;

    private IEnemyState currentState;                          // Current state object
    private EnemyState currentStateType = EnemyState.Patrol;   // Current state type (enum)

    /// <summary>
    /// Initializes the enemy, generates patrol points, and sets the initial state.
    /// </summary>
    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        Vector2 spawnPos = transform.position;

        // If no patrol points are set in the inspector, generate them randomly
        if (patrolPoints == null || patrolPoints.Length < 2)
        {
            patrolPoints = new Vector2[randomPatrolPointCount];
            patrolPoints[0] = spawnPos + Random.insideUnitCircle.normalized * patrolRadius;

            int maxAttempts = 20;
            for (int i = 1; i < randomPatrolPointCount; i++)
            {
                int attempts = 0;
                Vector2 candidate;
                do
                {
                    candidate = spawnPos + Random.insideUnitCircle.normalized * patrolRadius;
                    attempts++;
                }
                while (IsTooCloseToAny(candidate, patrolPoints, i, minPatrolPointDistance) && attempts < maxAttempts);

                patrolPoints[i] = candidate;
            }
        }

        SetState(new EnemyPatrolState(this, patrolPoints, moveSpeed), EnemyState.Patrol);
    }

    // Helper to check if a point is too close to any existing points
    private bool IsTooCloseToAny(Vector2 candidate, Vector2[] points, int count, float minDist)
    {
        for (int i = 0; i < count; i++)
        {
            if (Vector2.Distance(candidate, points[i]) < minDist)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Updates the current state and handles state transitions based on player distance.
    /// </summary>
    private void Update()
    {
        // Update the current state's behavior
        currentState?.Update();

        // Handle state transitions if the player exists
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            switch (currentStateType)
            {
                case EnemyState.Patrol:
                    // Switch to Chase if player is within chase radius
                    if (distance <= chaseRadius)
                        SetState(new EnemyChaseState(this, player, moveSpeed), EnemyState.Chase);
                    break;

                case EnemyState.Idle:
                    // Switch to Chase if player is within chase radius
                    if (distance <= chaseRadius)
                        SetState(new EnemyChaseState(this, player, moveSpeed), EnemyState.Chase);
                    break;

                case EnemyState.Chase:
                    // Return to Patrol if player is out of chase radius
                    if (distance > chaseRadius)
                        SetState(new EnemyPatrolState(this, patrolPoints, moveSpeed), EnemyState.Patrol);
                    // Switch to Attack if player is within attack range
                    else if (distance <= attackRange)
                        SetState(new EnemyAttackState(this), EnemyState.Attack);
                    break;

                case EnemyState.Attack:
                    // Return to Chase if player moves out of attack range
                    if (distance > attackRange)
                        SetState(new EnemyChaseState(this, player, moveSpeed), EnemyState.Chase);
                    break;
            }
        }
    }

    /// <summary>
    /// Changes the enemy's state, calling exit and enter methods as appropriate.
    /// </summary>
    /// <param name="newState">The new state object to enter.</param>
    /// <param name="stateType">The type of the new state.</param>
    public void SetState(IEnemyState newState, EnemyState stateType)
    {
        currentState?.Exit();
        currentState = newState;
        currentStateType = stateType;
        currentState?.Enter();
    }
}
