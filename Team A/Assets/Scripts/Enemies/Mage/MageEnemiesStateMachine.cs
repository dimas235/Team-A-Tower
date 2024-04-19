using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemiesStateMachine : MonoBehaviour
{
    public EnemiesState enemiesState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public EnemyMovement enemyMovement;
    public MageEnemiesThrowing mageEnemiesThrowingScript;

    void Start()
    {
        enemiesState = EnemiesState.Walking;
        if (mageEnemiesThrowingScript != null)
        {
            mageEnemiesThrowingScript.enabled = false;
        }
    }

    void Update()
    {
        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector2.left, out hit, detectionRange, defenderLayer);

        if (enemiesState == EnemiesState.Walking && defenderDetected)
        {
            ChangeState(EnemiesState.Shooting);
        }
        else if (!defenderDetected && enemiesState == EnemiesState.Shooting)
        {
            ChangeState(EnemiesState.Walking);
        }
    }

    void ChangeState(EnemiesState newState)
    {
        enemiesState = newState;
        mageEnemiesThrowingScript.enabled = (newState == EnemiesState.Shooting);
        if (enemyMovement != null)
        {
            enemyMovement.enabled = (newState == EnemiesState.Walking);
        }
    }

    public enum EnemiesState
    {
        Walking,
        Shooting
    }
}
