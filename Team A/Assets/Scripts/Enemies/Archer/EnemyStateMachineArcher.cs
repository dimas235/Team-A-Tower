using System.Collections;
using UnityEngine;

public class EnemyStateMachineArcher : MonoBehaviour
{
    public enum EnemiesState
    {
        Idle,
        Walking,
        Shooting
    }

    public EnemiesState enemiesState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public EnemyMovement enemyMovement;
    public Bow bowScript;
    private EnemyHealth enemyHealth;  // Referensi ke EnemyHealth
    private Animator animator;

    void Start()
    {
        enemiesState = EnemiesState.Idle;
        enemyHealth = GetComponent<EnemyHealth>();  // Ambil referensi dari parent
        animator = GetComponent<Animator>();
        if (bowScript != null)
        {
            bowScript.enabled = false;
        }
        UpdateAnimatorParameters(false, false, false);
    }

    void Update()
    {
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
        bowScript.enabled = (newState == EnemiesState.Shooting);
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
