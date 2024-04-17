using System.Collections;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    public GameObject magePrefab;
    public float attackCooldown;
    public LayerMask enemyLayer;
    public float detectionRadius = 10f;
    private PlayerMovement playerMovement;
    private float lastAttackTime;

    void Start()
    {
        lastAttackTime = attackCooldown;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        lastAttackTime -= Time.deltaTime;
        if (lastAttackTime <= 0 && !playerMovement.isMoving)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Vector3 enemyPosition = nearestEnemy.transform.position;
                EnsureFacingEnemy(enemyPosition);
                ShootTowards(enemyPosition);
                lastAttackTime = attackCooldown;
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        Collider[] enemiesDetected = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        GameObject nearestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        
        foreach(Collider potentialTarget in enemiesDetected)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestEnemy = potentialTarget.gameObject;
            }
        }

        return nearestEnemy;
    }

    void EnsureFacingEnemy(Vector3 enemyPosition)
    {
        bool shouldFaceRight = enemyPosition.x > transform.position.x;
        if (shouldFaceRight != playerMovement.facingRight)
        {
            playerMovement.Flip();
        }
    }

    void ShootTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        GameObject mageInstance = Instantiate(magePrefab, transform.position, Quaternion.identity);
        mageInstance.GetComponent<PlayerMage>().Initialize(direction);
    }

    IEnumerator EnableColliderAfterDelay(GameObject mage)
    {
        yield return new WaitForSeconds(0.1f);
        if (mage != null)
        {
            Collider mageCollider = mage.GetComponent<Collider>();
            if (mageCollider != null)
            {
                mageCollider.enabled = true;
            }
        }
    }
}
