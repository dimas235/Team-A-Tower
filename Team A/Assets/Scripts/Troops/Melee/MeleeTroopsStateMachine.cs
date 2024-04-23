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
    public LayerMask enemyLayer; // Layer untuk musuh
    // public LayerMask defenderLayer; // Layer untuk defender, digunakan untuk mengabaikan tabrakan
    // public float nonCollisionRadius = 1f; // Jarak untuk mengabaikan tabrakan antar musuh
    public DefenderMovement troopMovement;
    public TroopAttack troopAttack; // Class yang mirip dengan EnemyAttack tetapi untuk Troops
    private DefenderHealth troopHealth; // Menambahkan referensi ke TroopHealth
    void Start()
    {
        currentState = State.Walking;
        troopHealth = GetComponent<DefenderHealth>(); // Menginisialisasi troopHealth
        troopHealth.OnStunEnded += ReactiveAfterStun;
        if (troopAttack != null)
        {
            troopAttack.enabled = false;
        }
    }

    void Update()
    {
        if (troopHealth.isStunned)
        {
            // Jika troop ter-stun, nonaktifkan serangan dan gerakan, serta tetap dalam status ini
            troopAttack.enabled = false;
            troopMovement.enabled = false;
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
        bool enemyDetected = Physics.Raycast(transform.position, Vector2.right, out hit, detectionRange, enemyLayer);

        if (currentState == State.Walking && enemyDetected)
        {
            currentState = State.Attacking;
            troopAttack.enabled = true;
            troopMovement.enabled = false;
            ChangeState(State.Attacking);
        }
        else if (!enemyDetected && currentState == State.Attacking)
        {
            currentState = State.Walking;
            troopAttack.enabled = false;
            troopMovement.enabled = true;
            ChangeState(State.Walking);
        }
    }

    private void ReactiveAfterStun()
    {
        troopAttack.enabled = true;
    }

    void ChangeState(State newState)
    {
        currentState = newState;
        troopAttack.enabled = (newState == State.Attacking);
        if (troopMovement != null)
        {
            troopMovement.enabled = (newState == State.Walking);
        }
    }



    // void IgnoreCollisionsWithDefender()
    // {
    //     // Mendeteksi musuh lain dalam radius tertentu
    //     Collider[] enemies = Physics.OverlapSphere(transform.position, nonCollisionRadius, defenderLayer);
    //     foreach (var otherEnemy in enemies)
    //     {
    //         if (otherEnemy.gameObject != gameObject) // Pastikan tidak memilih collider sendiri
    //         {
    //             Physics.IgnoreCollision(GetComponent<Collider>(), otherEnemy, true);
    //         }
    //     }
    // }
}
