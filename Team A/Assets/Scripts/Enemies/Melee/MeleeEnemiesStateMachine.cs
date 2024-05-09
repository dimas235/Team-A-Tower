using System.Collections;
using UnityEngine;

public class MeleeEnemiesStateMachine : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking,
        Stunned
    };

    public State currentState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public EnemyMovement enemyMovement;
    public EnemyAttack enemyAttack;
    public Animator animator;
    public EnemyHealth enemyHealth;

    private float stunDuration;
    private bool isStunned;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        currentState = State.Walking;
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsStunned", false);
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
    }

    void Update()
    {
        if (currentState == State.Stunned)
        {
            if (stunDuration > 0)
            {
                stunDuration -= Time.deltaTime;
            }
            else
            {
                isStunned = false;
                animator.SetBool("IsStunned", false);
                TransitionToWalking(); // Or any other appropriate state
            }
        }
        else if (!isStunned)
        {
            CheckStateConditions();
        }
    }

    public void ApplyStun(float duration)
    {
        if (currentState != State.Stunned && enemyHealth.isAlive)
        {
            isStunned = true;
            stunDuration = duration;
            TransitionToStunned();
        }
    }

    private void TransitionToStunned()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsStunned", true);
        currentState = State.Stunned;
        enemyAttack.enabled = false;
        enemyMovement.enabled = false;
    }

    void CheckStateConditions()
    {
        if (!isStunned)
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * detectionRange;
            bool defenderDetected = Physics.Raycast(transform.position, forward, out hit, detectionRange, defenderLayer);

            if (defenderDetected)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("TowerDefense") ||
                    hit.collider.gameObject.layer == LayerMask.NameToLayer("Troops") ||
                    hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (currentState != State.Attacking || animator.GetBool("IsCooldown"))
                    {
                        TransitionToAttack();
                    }
                }
            }
            else
            {
                TransitionToWalking();
            }
            if (currentState == State.Attacking && enemyAttack.IsAttackOnCooldown())
            {
                animator.SetBool("IsCooldown", true);
            }
            else
            {
                animator.SetBool("IsCooldown", false);
            }
        }
    }

    private void TransitionToAttack()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttack", true);
        animator.SetBool("IsCooldown", enemyAttack.IsAttackOnCooldown());
        currentState = State.Attacking;
        enemyAttack.enabled = true;
        enemyMovement.enabled = false;
    }

    private void TransitionToWalking()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsCooldown", false);
        animator.SetBool("IsStunned", false);
        currentState = State.Walking;
        enemyAttack.enabled = false;
        enemyMovement.enabled = true;
    }
}
