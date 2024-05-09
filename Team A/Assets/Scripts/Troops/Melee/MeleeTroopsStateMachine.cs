using System.Collections;
using UnityEngine;

public class MeleeTroopsStateMachine : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking
    };

    public State currentState;
    public float detectionRange;
    public LayerMask enemyLayer;
    public DefenderMovement troopMovement;
    public TroopAttack troopAttack;
    public Animator animator;
    

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

        if (enemyDetected)
        {
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
