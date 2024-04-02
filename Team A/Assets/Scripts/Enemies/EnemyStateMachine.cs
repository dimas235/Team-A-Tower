using System.Collections;
using UnityEngine;

public class EnemiesStateMachine : MonoBehaviour
{
    public EnemyState enemyState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public LayerMask enemyLayer; // Layer untuk musuh lain
    public float nonCollisionRadius = 1f; // Jarak untuk mengabaikan tabrakan antar musuh
    public EnemyMovement enemyMovement;
    public EnemyAttack enemyAttack;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Walking;
        if(enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector2.left, out hit, detectionRange, defenderLayer);

        if(enemyState == EnemyState.Walking && defenderDetected)
        {
            enemyState = EnemyState.Attacking;
            enemyAttack.enabled = true;
            enemyMovement.enabled = false;
        }
        else if (!defenderDetected && enemyState == EnemyState.Attacking)
        {
            enemyState = EnemyState.Walking;
            enemyAttack.enabled = false;
            enemyMovement.enabled = true;
        }

        IgnoreCollisionsWithEnemies();
    }

    void IgnoreCollisionsWithEnemies()
    {
        // Mendeteksi musuh lain dalam radius tertentu
        Collider[] enemies = Physics.OverlapSphere(transform.position, nonCollisionRadius, enemyLayer);
        foreach (var otherEnemy in enemies)
        {
            if (otherEnemy.gameObject != gameObject) // Pastikan tidak memilih collider sendiri
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), otherEnemy, true);
            }
        }
    }

    public enum EnemyState
    {
        Walking,
        Attacking
    };
}
