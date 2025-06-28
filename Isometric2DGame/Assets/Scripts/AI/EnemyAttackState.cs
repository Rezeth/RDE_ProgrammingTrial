using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private readonly EnemyAI enemyAI;

    public EnemyAttackState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        AIStateLoggingManager.LogStateEnter("Attack");
    }

    public void Update()
    {
        AIStateLoggingManager.Log("Enemy is attacking the player...");
    }

    public void Exit()
    {
        AIStateLoggingManager.LogStateExit("Attack");
    }
}
