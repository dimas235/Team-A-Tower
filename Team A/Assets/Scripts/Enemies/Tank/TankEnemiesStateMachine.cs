using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemiesStateMachine : MonoBehaviour
{
   public enum State
   {
       Walking,
       Attacking
   };

    public State currentState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public EnemyMovement enemyMovement;
    public TankEnemiesAttack tankEnemiesAttack;

    void Start()
    {
        currentState = State.Walking;
        if (tankEnemiesAttack != null)
        {
            tankEnemiesAttack.enabled = false;
        }
    }

    void Update()
    {
        CheckStateConditions();
    }

    void CheckStateConditions()
    {
        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector2.left, out hit, detectionRange, defenderLayer);

        if (currentState == State.Walking && defenderDetected)
        {
            currentState = State.Attacking;
            tankEnemiesAttack.enabled = true;
            enemyMovement.enabled = false;
        }
        else if (!defenderDetected && currentState == State.Attacking)
        {
            currentState = State.Walking;
            tankEnemiesAttack.enabled = false;
            enemyMovement.enabled = true;
        }
    }
}
