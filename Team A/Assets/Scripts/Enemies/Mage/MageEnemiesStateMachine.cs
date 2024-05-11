using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemiesStateMachine : MonoBehaviour
{
    public enum EnemiesState
    {
        idle,
        Walking,
        Shooting
    }

    public EnemiesState enemiesState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public EnemyMovement enemyMovement;
    public MageEnemiesThrowing mageEnemiesThrowingScript;
    private EnemyHealth enemyHealth;  // Referensi ke EnemyHealth
    private Animator animator;

    void Start()
    {
        enemiesState = EnemiesState.idle;
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        if (mageEnemiesThrowingScript != null)
        {
            mageEnemiesThrowingScript.enabled = false;
        }
        UpdateAnimatorParameters(false, false, false);
    }

    void Update()
    {
        if (enemyHealth.isStunned)
        {
            UpdateAnimatorParameters(false, false, false);
            return;
        }

        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector3.left, out hit, detectionRange, defenderLayer);

        if (defenderDetected && enemiesState != EnemiesState.Shooting)
        {
            ChangeState(EnemiesState.Shooting);
        }
        else if (!defenderDetected && enemiesState == EnemiesState.Shooting)
        {
            ChangeState(EnemiesState.Walking);
        }
        else if (!defenderDetected && enemiesState != EnemiesState.Walking)
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
            enemyMovement.enabled = (newState != EnemiesState.Shooting);
        }

        bool isCooldown = animator.GetBool("IsCooldown");
        UpdateAnimatorParameters(newState == EnemiesState.Shooting, newState == EnemiesState.Walking, isCooldown);
    }

    void UpdateAnimatorParameters(bool isAttacking, bool isRunning, bool isCooldown)
    {
        animator.SetBool("IsAttack", isAttacking);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsCooldown", isCooldown);
    }
}