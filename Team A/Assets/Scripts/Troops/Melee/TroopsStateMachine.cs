using UnityEngine;

public class TroopsStateMachine : BaseStateMachine
{
    public DefenderMovement troopMovement;
    public TroopAttack troopAttack; // Asumsikan ini adalah class yang mirip dengan EnemyAttack tetapi untuk Troops

    protected override void Start()
    {
        currentState = State.Walking;
        if (troopAttack != null)
        {
            troopAttack.enabled = false;
        }
    }

    protected override void CheckStateConditions()
    {
        RaycastHit hit;
        // Asumsikan bahwa Troops menyerang musuh dari arah yang berlawanan atau memiliki logika deteksi target yang berbeda
        bool enemyDetected = Physics.Raycast(transform.position, Vector2.right, out hit, detectionRange, enemyLayer);

        if (currentState == State.Walking && enemyDetected)
        {
            currentState = State.Attacking;
            troopAttack.enabled = true;
            troopMovement.enabled = false;
        }
        else if (!enemyDetected && currentState == State.Attacking)
        {
            currentState = State.Walking;
            troopAttack.enabled = false;
            troopMovement.enabled = true;
        }
    }
}
