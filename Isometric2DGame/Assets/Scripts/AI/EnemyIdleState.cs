using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private readonly EnemyAI enemyAI;

    public EnemyIdleState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Idle");
    }

    public void Update()
    {
        AIStateLoggingManager.Log("Enemy is idling...");
    }

    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Idle");
    }
}
