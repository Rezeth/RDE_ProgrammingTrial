using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float chaseRadius = 5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform player;

    private IEnemyState currentState;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        SetState(new EnemyIdleState(this));

    }

    private void Update()
    {
        currentState?.Update();

        // State transition logic
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (currentState is EnemyIdleState && distance <= chaseRadius)
            {
                SetState(new EnemyChaseState(this, player, moveSpeed));
            }
            else if (currentState is EnemyChaseState && distance > chaseRadius)
            {
                SetState(new EnemyIdleState(this));

            }
        }
    }

    public void SetState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
