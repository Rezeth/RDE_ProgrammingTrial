using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float chaseRadius = 5f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform player;

    private IEnemyState currentState;
    private EnemyState currentStateType = EnemyState.Idle;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        SetState(new EnemyIdleState(this), EnemyState.Idle);
    }

    private void Update()
    {
        currentState?.Update();

        // State transition logic
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            switch (currentStateType)
            {
                case EnemyState.Idle:
                    if (distance <= chaseRadius)
                        SetState(new EnemyChaseState(this, player, moveSpeed), EnemyState.Chase);
                    break;

                case EnemyState.Chase:
                    if (distance > chaseRadius)
                        SetState(new EnemyIdleState(this), EnemyState.Idle);
                    else if (distance <= attackRange)
                        SetState(new EnemyAttackState(this), EnemyState.Attack);
                    break;

                case EnemyState.Attack:
                    if (distance > attackRange)
                        SetState(new EnemyChaseState(this, player, moveSpeed), EnemyState.Chase);
                    break;
            }
        }
    }

    public void SetState(IEnemyState newState, EnemyState stateType)
    {
        currentState?.Exit();
        currentState = newState;
        currentStateType = stateType;
        currentState?.Enter();
    }
}
