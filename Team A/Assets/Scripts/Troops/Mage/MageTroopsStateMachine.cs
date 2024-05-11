using System.Collections;
using UnityEngine;

public class MageTroopsStateMachine : MonoBehaviour
{
    public enum TroopsState
    {
        idle,
        Walking,
        Shooting
    }

    public TroopsState troopsState;
    public float detectionRange;
    public LayerMask enemyLayer;
    public MageTroopsThrowing mageTroopsThrowingScript;
    public DefenderMovement defenderMovement;
    private DefenderHealth defenderHealth;
    private Animator animator;

    void Start()
    {
        troopsState = TroopsState.idle;
        defenderHealth = GetComponent<DefenderHealth>();
        animator = GetComponent<Animator>();
        if (mageTroopsThrowingScript != null)
        {
            mageTroopsThrowingScript.enabled = false;
        }
        UpdateAnimatorParameters(false, false, false);
    }

    void Update()
    {
        if (defenderHealth.isStunned)
        {
            UpdateAnimatorParameters(false, false, false);
            return;
        }

        RaycastHit hit;
        bool enemyDetected = Physics.Raycast(transform.position, Vector3.right, out hit, detectionRange, enemyLayer);

        if (enemyDetected && troopsState != TroopsState.Shooting)
        {
            ChangeState(TroopsState.Shooting);
        }
        else if (!enemyDetected && troopsState == TroopsState.Shooting)
        {
            ChangeState(TroopsState.Walking);
        }
        else if (!enemyDetected && troopsState != TroopsState.Walking)
        {
            ChangeState(TroopsState.Walking);
        }
    }

    void ChangeState(TroopsState newState)
    {
        troopsState = newState;
        mageTroopsThrowingScript.enabled = (newState == TroopsState.Shooting);
        if (defenderMovement != null)
        {
            defenderMovement.enabled = (newState != TroopsState.Shooting);
        }

        bool isCooldown = animator.GetBool("IsCooldown");
        UpdateAnimatorParameters(newState == TroopsState.Shooting, newState == TroopsState.Walking, isCooldown);
    }

    void UpdateAnimatorParameters(bool isAttacking, bool isRunning, bool isCooldown)
    {
        animator.SetBool("IsAttack", isAttacking);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsCooldown", isCooldown);
    }
}
