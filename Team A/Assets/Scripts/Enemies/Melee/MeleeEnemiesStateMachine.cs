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
    // public LayerMask enemiesLayer;
    public LayerMask defenderLayer; // Layer untuk defender, digunakan untuk mengabaikan tabrakan
    // public float nonCollisionRadius = 1f; // Jarak untuk mengabaikan tabrakan antar musuh
    public EnemyMovement enemyMovement;
    public EnemyAttack enemyAttack;

    void Start()
    {
        currentState = State.Walking;
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
    }

    void Update()
    {
        CheckStateConditions();
        // IgnoreCollisionsWithEnemies();
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
        }
        else if (!defenderDetected && currentState == State.Attacking)
        {
            currentState = State.Walking;
            enemyAttack.enabled = false;
            enemyMovement.enabled = true;
        }
    }

    // void IgnoreCollisionsWithEnemies()
    // {
    //     // Mendeteksi musuh lain dalam radius tertentu
    //     Collider[] enemies = Physics.OverlapSphere(transform.position, nonCollisionRadius, enemiesLayer);
    //     foreach (var otherEnemy in enemies)
    //     {
    //         if (otherEnemy.gameObject != gameObject) // Pastikan tidak memilih collider sendiri
    //         {
    //             Physics.IgnoreCollision(GetComponent<Collider>(), otherEnemy, true);
    //         }
    //     }
    // }
}
