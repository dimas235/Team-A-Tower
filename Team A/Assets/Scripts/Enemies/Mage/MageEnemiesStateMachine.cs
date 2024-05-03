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
    private EnemyHealth enemyHealth;  // Referensi ke EnemyHealth

    void Start()
    {
        enemiesState = EnemiesState.Walking;
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.OnStunEnded += HandleStunEnded;  // Subscribe ke event OnStunEnded
        if (mageEnemiesThrowingScript != null)
        {
            mageEnemiesThrowingScript.enabled = false;
        }
    }

    void Update()
    {
        if (enemyHealth.isStunned)
        {
            mageEnemiesThrowingScript.enabled = false;  // Nonaktifkan mageEnemiesThrowingScript jika musuh ter-stun
            enemyMovement.SetMovement(false);  // Nonaktifkan enemyMovement jika musuh ter-stun
            return;  // Keluar dari update jika musuh ter-stun
        }

        Vector3 raycastDirection = transform.TransformDirection(Vector2.left);
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
       mageEnemiesThrowingScript.enabled = newState == EnemiesState.Shooting;
        enemyMovement.SetMovement(newState == EnemiesState.Walking);
    }

    public enum EnemiesState
    {
        Walking,
        Shooting
    }

    private void HandleStunEnded()
    {
        if (Physics.Raycast(transform.position, Vector2.left, detectionRange, defenderLayer))
        {
            ChangeState(EnemiesState.Shooting);
        }
    }

    void OnDestroy()
    {
        enemyHealth.OnStunEnded -= HandleStunEnded;  // Unsubscribe dari event OnStunEnded
    }
}
