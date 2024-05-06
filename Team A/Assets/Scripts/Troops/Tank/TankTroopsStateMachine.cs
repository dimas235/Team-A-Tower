using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTroopsStateMachine : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking
    };

    public State currentState;
    public Animator animator;
    public float detectionRange;
    public LayerMask enemyLayer; // Layer untuk musuh
    public DefenderMovement troopMovement;
    public TankTroopsAttack troopAttack; // Class yang mirip dengan EnemyAttack tetapi untuk Troops

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = State.Walking;
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttacking", false);
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
        bool enemyDetected = Physics.Raycast(transform.position, Vector2.right, out hit, detectionRange, enemyLayer);

        if (currentState == State.Walking && enemyDetected)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", true);
            currentState = State.Attacking;
            troopAttack.enabled = true;
            troopMovement.enabled = false;
        }
        else if (!enemyDetected && currentState == State.Attacking)
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsAttacking", false);
            currentState = State.Walking;
            troopAttack.enabled = false;
            troopMovement.enabled = true;
        }
    }
}
