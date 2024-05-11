using UnityEngine;

public class MeleeEnemiesStateMachine : MonoBehaviour
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
    public EnemyAttack enemyAttack;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = State.Walking;
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsCooldown", false);
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
    }

    void Update()
    {
        CheckStateConditions();
    }

    void CheckStateConditions()
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
        currentState = State.Walking;
        enemyAttack.enabled = false;
        enemyMovement.enabled = true;
    }
}
