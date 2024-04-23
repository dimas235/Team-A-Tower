using System.Collections;
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
    private EnemyHealth enemyHealth;  // Menambahkan referensi ke EnemyHealth

    void Start()
    {
        currentState = State.Walking;
        enemyHealth = GetComponent<EnemyHealth>();  // Menginisialisasi enemyHealth
        enemyHealth.OnStunEnded += ReactiveAfterStun;
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
    }

    void Update()
    {
        if (enemyHealth.isStunned)
        {
            // Jika musuh ter-stun, nonaktifkan serangan dan gerakan, serta tetap dalam status ini
            enemyAttack.enabled = false;
            enemyMovement.enabled = false;
            return;
        }
        else
        {
            CheckStateConditions();
        }
    }

    void CheckStateConditions()
    {
        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector2.left, out hit, detectionRange, defenderLayer);

        if (currentState == State.Walking && defenderDetected)
        {
            currentState = State.Attacking;
            enemyAttack.enabled = true;
            enemyMovement.enabled = false;
            ChangeState(State.Attacking);
        }
        else if (!defenderDetected && currentState == State.Attacking)
        {
            currentState = State.Walking;
            enemyAttack.enabled = false;
            enemyMovement.enabled = true;
            ChangeState(State.Walking);
        }
    }

    private void ReactiveAfterStun()
    {
        enemyAttack.enabled = true;
    }

    void ChangeState(State newState)
    {
        currentState = newState;
        enemyAttack.enabled = (newState == State.Attacking);
        if (enemyMovement != null)
        {
            enemyMovement.enabled = (newState == State.Walking);
        }
    }
}
