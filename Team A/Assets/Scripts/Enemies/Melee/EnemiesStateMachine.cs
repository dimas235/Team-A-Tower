using UnityEngine;

public class EnemiesStateMachine : BaseStateMachine
{
    public EnemyMovement enemyMovement;
    public EnemyAttack enemyAttack;

    protected override void Start()
    {
        currentState = State.Walking;
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
    }

    protected override void CheckStateConditions()
    {
        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector2.left, out hit, detectionRange, defenderLayer);

        if (currentState == State.Walking && defenderDetected)
        {
            currentState = State.Attacking;
            enemyAttack.enabled = true;
            enemyMovement.enabled = false;
        }
        else if (!defenderDetected && currentState == State.Attacking)
        {
            currentState = State.Walking;
            enemyAttack.enabled = false;
            enemyMovement.enabled = true;
        }
    }
}
