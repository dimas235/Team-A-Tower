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
        enemyMovement.SetMovement(true);  // Enable movement initially
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
            ChangeState(State.Attacking);
        }
        else if (!defenderDetected && currentState == State.Attacking)
        {
            ChangeState(State.Walking);
        }
    }

    void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        switch (currentState)
        {
            case State.Walking:
                tankEnemiesAttack.enabled = false;
                enemyMovement.SetMovement(true);
                break;
            case State.Attacking:
                tankEnemiesAttack.enabled = true;
                enemyMovement.SetMovement(false);
                break;
        }
    }
}
