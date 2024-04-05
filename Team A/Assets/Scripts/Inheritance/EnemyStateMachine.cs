using System.Collections;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    public enum State
    {
        Walking,
        Attacking
    };

    public State currentState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public LayerMask enemyLayer; // Layer untuk musuh lain
    public float nonCollisionRadius = 1f; // Jarak untuk mengabaikan tabrakan antar musuh
    protected virtual void Start() {}

    protected virtual void Update()
    {
        CheckStateConditions();
        IgnoreCollisionsWithEnemies();
    }

    protected abstract void CheckStateConditions();

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
}
