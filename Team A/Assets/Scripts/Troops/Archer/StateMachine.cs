using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public float detectionRange;
    public LayerMask enemyLayer;
    public DefenderMovement defenderMovement;
    public Throwing throwingScript;
    private DefenderHealth defenderHealth;
    private Animator animator;

    public enum TroopsState
    {
        Idle,
        Walking,
        Shooting
    }

    public TroopsState troopsState;

    void Start()
    {
        troopsState = TroopsState.Idle;
        defenderHealth = GetComponent<DefenderHealth>();
        animator = GetComponent<Animator>();
        if (throwingScript != null)
        {
            throwingScript.enabled = false;
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
        throwingScript.enabled = (newState == TroopsState.Shooting);
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