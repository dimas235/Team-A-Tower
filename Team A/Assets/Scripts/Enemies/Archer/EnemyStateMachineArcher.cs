using System.Collections;
using UnityEngine;

public class EnemyStateMachineArcher : MonoBehaviour
{
    public enum EnemiesState
    {
        Walking,
        Shooting
    }

    public EnemiesState enemiesState;
    public float detectionRange;
    public LayerMask defenderLayer;
    public EnemyMovement enemyMovement;
    public Bow bowScript;
    private EnemyHealth enemyHealth;  // Referensi ke EnemyHealth

    void Start()
    {
        enemiesState = EnemiesState.Walking;
        enemyHealth = GetComponent<EnemyHealth>();  // Inisialisasi EnemyHealth
        if (bowScript != null)
        {
            bowScript.enabled = false;
        }
    }

    void Update()
    {
        if (enemyHealth.isStunned)
        {
            bowScript.enabled = false;  // Nonaktifkan bowScript jika musuh ter-stun
            enemyMovement.enabled = false;  // Nonaktifkan pergerakan jika musuh ter-stun
            return;  // Keluar dari update jika musuh ter-stun
        }

        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, Vector2.left, out hit, detectionRange, defenderLayer);

        if (enemiesState == EnemiesState.Walking && defenderDetected)
        {
            ChangeState(EnemiesState.Shooting);
        }
        else if (!defenderDetected && enemiesState == EnemiesState.Shooting)
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
            enemyMovement.enabled = (newState == EnemiesState.Walking);
        }
    }
}
