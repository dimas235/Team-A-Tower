using UnityEngine;

public class TankTroopsStateMachine : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking,
        // Idle
    };

    public State currentState;
    public Animator animator;
    public float detectionRange;
    public LayerMask enemyLayer;
    public DefenderMovement troopMovement;
    public TankTroopsAttack troopAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = State.Walking;
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttack", false);
        if (troopAttack != null)
        {
            troopAttack.enabled = false;
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
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("TowerEnemy") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
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

        if (currentState == State.Attacking && troopAttack.IsAttackOnCooldown())
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
        animator.SetBool("IsCooldown", troopAttack.IsAttackOnCooldown());
        currentState = State.Attacking;
        troopAttack.enabled = true;
        troopMovement.enabled = false;
    }

    // private void TransitionToIdle()
    // {
    //     animator.SetBool("IsRunning", false);
    //     animator.SetBool("IsAttack", false);
    //     animator.SetBool("IsCooldown", true);
    //     currentState = State.Idle;
    //     troopAttack.enabled = false;
    //     troopMovement.enabled = false;
    // }

    private void TransitionToWalking()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsCooldown", false);
        currentState = State.Walking;
        troopAttack.enabled = false;
        troopMovement.enabled = true;
    }
}
