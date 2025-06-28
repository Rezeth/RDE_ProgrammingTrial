using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private readonly EnemyAI enemyAI;
    private readonly Transform player;
    private readonly float moveSpeed;

    public EnemyChaseState(EnemyAI enemyAI, Transform player, float moveSpeed)
    {
        this.enemyAI = enemyAI;
        this.player = player;
        this.moveSpeed = moveSpeed;
    }

    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Chase");
    }

    public void Update()
    {
        if (player == null) return;

        // Move towards the player
        Vector2 direction = (player.position - enemyAI.transform.position).normalized;
        enemyAI.transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        AIStateLoggingManager.Log("Enemy is chasing the player...");
    }

    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Chase");
    }
}
