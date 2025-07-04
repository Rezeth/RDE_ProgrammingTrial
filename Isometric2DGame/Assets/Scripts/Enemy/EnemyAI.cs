using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Controls the enemy's AI state machine, handling transitions between different states,
/// patrol logic, state-based sprite coloring, and self-healing.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Tooltip("Distance at which the enemy starts chasing the player")]
    [SerializeField] private float chaseRadius = 5f;

    [Tooltip("Patrol points for the enemy to move between. If empty, random points will be generated around spawn.")]
    [SerializeField] private Vector2[] patrolPoints;

    [Tooltip("Number of random patrol points to generate if none are set in the inspector.")]
    [SerializeField] private int randomPatrolPointCount = 2;

    [Tooltip("Radius around spawn for generating patrol points")]
    [SerializeField] private float patrolRadius = 3f;

    [Tooltip("Minimum distance between patrol points")]
    [SerializeField] private float minPatrolPointDistance = 1.5f;

    [Tooltip("Time (in seconds) the enemy idles after losing sight of the player before resuming patrol.")]
    [SerializeField] private float idleAfterChaseDuration = 5f;

    [Tooltip("Reference to the player transform, defaults to an object with the 'Player' tag")]
    [SerializeField] private Transform player;

    /// <summary>
    /// Public property to access the player transform.
    /// </summary>
    public Transform Player => player;

    // Reference to the enemy's stats (health, speed, attack, etc.)
    private EnemyStats stats;
    // The current state object in the state machine
    private IEnemyState currentState;
    // The current state type (enum) for logic branching
    private EnemyState currentStateType = EnemyState.Patrol;
    // Reference to the enemy's SpriteRenderer for visual feedback
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Initializes references to EnemyStats and SpriteRenderer.
    /// </summary>
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initializes the enemy, generates patrol points if needed, and sets the initial state.
    /// </summary>
    private void Start()
    {
        // Find the player if not assigned in the inspector
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Generate patrol points if not set in the inspector
        if (patrolPoints == null || patrolPoints.Length < 2)
        {
            GenerateRandomPatrolPoints();
        }

        // Start in patrol state
        SetState(new EnemyPatrolState(this, patrolPoints, stats.MoveSpeed), EnemyState.Patrol);
    }

    /// <summary>
    /// Checks if a candidate patrol point is too close to any existing points.
    /// </summary>
    /// <param name="candidate">The candidate point to check.</param>
    /// <param name="points">Array of existing points.</param>
    /// <param name="count">Number of points to check against.</param>
    /// <param name="minDist">Minimum allowed distance.</param>
    /// <returns>True if too close to any point, false otherwise.</returns>
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
    /// Updates the current state, handles self-healing, and manages state transitions based on player distance.
    /// </summary>
    private void Update()
    {
        // Update the current state's behavior
        currentState?.Update();

        // Enemy self-healing logic (if enabled in stats)
        stats.TryHeal();

        // Handle state transitions if the player exists
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            switch (currentStateType)
            {
                case EnemyState.Patrol:
                    // Switch to Chase if player is within chase radius
                    if (distance <= chaseRadius)
                        SetState(new EnemyChaseState(this, player, stats.MoveSpeed), EnemyState.Chase);
                    break;

                case EnemyState.Idle:
                    // Switch to Chase if player is within chase radius
                    if (distance <= chaseRadius)
                        SetState(new EnemyChaseState(this, player, stats.MoveSpeed), EnemyState.Chase);
                    break;

                case EnemyState.Chase:
                    // Return to Patrol if player is out of chase radius
                    if (distance > chaseRadius)
                    {
                        // Enter idle, then patrol after idle duration
                        SetState(
                            new EnemyIdleState(this, idleAfterChaseDuration, () =>
                            {
                                GenerateRandomPatrolPoints();
                                SetState(new EnemyPatrolState(this, patrolPoints, stats.MoveSpeed), EnemyState.Patrol);
                            }),
                            EnemyState.Idle
                        );
                    }
                    // Switch to Attack if player is within attack range
                    else if (distance <= stats.AttackRange)
                        SetState(new EnemyAttackState(this), EnemyState.Attack);
                    break;

                case EnemyState.Attack:
                    // Return to Chase if player moves out of attack range
                    if (distance > stats.AttackRange)
                        SetState(new EnemyChaseState(this, player, stats.MoveSpeed), EnemyState.Chase);
                    break;
            }
        }
    }

    /// <summary>
    /// Changes the enemy's state, calling exit and enter methods as appropriate, and updates sprite color.
    /// </summary>
    /// <param name="newState">The new state object to enter.</param>
    /// <param name="stateType">The type of the new state.</param>
    public void SetState(IEnemyState newState, EnemyState stateType)
    {
        currentState?.Exit();
        currentState = newState;
        currentStateType = stateType;
        UpdateSpriteColor(stateType);
        currentState?.Enter();
    }

    /// <summary>
    /// Updates the sprite color based on the current state of the enemy for visual feedback.
    /// </summary>
    /// <param name="state">The current enemy state.</param>
    private void UpdateSpriteColor(EnemyState state)
    {
        if (spriteRenderer == null)
            return;

        switch (state)
        {
            case EnemyState.Patrol:
                spriteRenderer.color = Color.white;
                break;
            case EnemyState.Chase:
                spriteRenderer.color = Color.red;
                break;
            case EnemyState.Attack:
                spriteRenderer.color = Color.black;
                break;
            case EnemyState.Idle:
                spriteRenderer.color = Color.green;
                break;
        }
    }

    /// <summary>
    /// Generates random patrol points around the enemy's spawn position, ensuring minimum distance between points.
    /// </summary>
    private void GenerateRandomPatrolPoints()
    {
        Vector2 spawnPos = transform.position;
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
}
