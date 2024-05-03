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
    private EnemyHealth enemyHealth;  // Referensi ke EnemyHealth

    void Start()
    {
        currentState = State.Walking;
        enemyHealth = GetComponent<EnemyHealth>();  // Inisialisasi enemyHealth
        enemyHealth.OnStunEnded += ReactiveAfterStun;
        enemyAttack.enabled = false;
        enemyMovement.SetMovement(true);  // Mengaktifkan gerakan di awal
    }

    void Update()
    {
        if (enemyHealth.isStunned)
        {
            enemyAttack.enabled = false;
            enemyMovement.SetMovement(false);  // Menonaktifkan gerakan ketika ter-stun
            return;
        }
        
        CheckStateConditions();
    }

    void CheckStateConditions()
    {
        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector2.left, out hit, detectionRange, defenderLayer);

        if (currentState == State.Walking && defenderDetected)
        {
            ChangeState(State.Attacking);
        }
        else if (!defenderDetected && currentState == State.Attacking)
        {
            ChangeState(State.Walking);
        }
    }

    void ChangeState(State newState)
    {
        if (currentState == newState) return; // Hindari perubahan state yang berulang

        currentState = newState;
        switch (newState)
        {
            case State.Walking:
                enemyAttack.enabled = false;
                enemyMovement.SetMovement(true);  // Mengaktifkan gerakan
                break;
            case State.Attacking:
                enemyAttack.enabled = true;
                enemyMovement.SetMovement(false);  // Menonaktifkan gerakan
                break;
        }
    }

    private void ReactiveAfterStun()
    {
        // Logika reaktivasi setelah ter-stun
        if (Physics.Raycast(transform.position, Vector2.left, detectionRange, defenderLayer))
        {
            ChangeState(State.Attacking);
        }
        else
        {
            ChangeState(State.Walking);
        }
    }

    void OnDestroy()
    {
        // Bersihkan untuk menghindari kebocoran memori
        if (enemyHealth != null)
        {
            enemyHealth.OnStunEnded -= ReactiveAfterStun;
        }
    }
}
