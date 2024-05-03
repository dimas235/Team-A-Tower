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
        enemyHealth = GetComponent<EnemyHealth>();
        if (bowScript != null)
        {
            bowScript.enabled = false;
        }
    }

    void Update()
    {
        if (enemyHealth.isStunned)
        {
            bowScript.enabled = false;
            enemyMovement.SetMovement(false);
            return;
        }

        Vector3 raycastDirection = transform.TransformDirection(Vector2.left);
        RaycastHit hit;
        bool defenderDetected = Physics.Raycast(transform.position, raycastDirection, out hit, detectionRange, defenderLayer);
        Debug.DrawRay(transform.position, raycastDirection * detectionRange, Color.red);

        if (hit.collider != null) // Cek jika ada yang terkena raycast
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
        }

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
        enemyMovement.SetMovement(newState == EnemiesState.Walking);
    }
}
