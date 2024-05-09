using UnityEngine;

public class TankEnemiesStateMachine : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking,
        Idle
    };

    public State currentState;
    public Animator animator;
    public float detectionRange;
    public LayerMask enemyLayer;
    public EnemyMovement enemyMovement;
    public TankEnemiesAttack tankEnemiesAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = State.Walking;
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttack", false);
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
        Vector3 forward = transform.TransformDirection(Vector3.forward) * detectionRange;
        bool enemyDetected = Physics.Raycast(transform.position, forward, out hit, detectionRange, enemyLayer);
        Debug.DrawRay(transform.position, forward, Color.red, 1.0f);

        if (enemyDetected)
        {
            Debug.Log($"Detected: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            // Menambahkan pemeriksaan untuk tiga layer yang berbeda
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

        if (currentState == State.Attacking && tankEnemiesAttack.IsAttackOnCooldown())
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
        animator.SetBool("IsCooldown", tankEnemiesAttack.IsAttackOnCooldown());
        currentState = State.Attacking;
        tankEnemiesAttack.enabled = true;
        enemyMovement.enabled = false;
    }

    private void TransitionToIdle()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsCooldown", false);
        currentState = State.Idle;
        tankEnemiesAttack.enabled = false;
        enemyMovement.enabled = false;
    }

    private void TransitionToWalking()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsCooldown", false);
        currentState = State.Walking;
        tankEnemiesAttack.enabled = false;
        enemyMovement.enabled = true;
    }
}
