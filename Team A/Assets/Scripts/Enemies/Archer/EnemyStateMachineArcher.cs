using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachineArcher : MonoBehaviour
{
    public EnemiesState enemiesState;
    public float detectionRange;
    public LayerMask enemiesLayer;
    public LayerMask defenderLayer;
    public float nonCollisionRadius = 1f;
    public EnemyMovement enemyMovement;
    public Bow bowScript;

    void Start()
    {
        enemiesState = EnemiesState.Walking;
        if (bowScript != null)
        {
            bowScript.enabled = false;
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

        IgnoreCollisionsWithAttacker();
    }

    void ChangeState(EnemiesState newState)
    {
        enemiesState = newState;
        bowScript.enabled = (newState == EnemiesState.Shooting);
        if (enemyMovement != null)
        {
            enemyMovement.enabled = (newState == EnemiesState.Walking);
        }
    }

    void IgnoreCollisionsWithAttacker()
    {
        Collider[] attackers = Physics.OverlapSphere(transform.position, nonCollisionRadius, enemiesLayer);
        foreach (var otherAttacker in attackers)
        {
            if (otherAttacker.gameObject != gameObject)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), otherAttacker, true);
            }
        }
    }

    public enum EnemiesState
    {
        Walking,
        Shooting
    }
}
