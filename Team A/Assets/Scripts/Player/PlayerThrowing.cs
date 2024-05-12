using System.Collections;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    private bool projectileSpawned;
    public GameObject magePrefab;
    public float attackCooldown = 1.0f;  // Cooldown time in seconds
    public Vector3 projectileOffset = new Vector3(0f, 0.5f, 1.0f);  // Default offset

    private float nextAttackTime = 0f;
    private Animator animator;
    private PlayerMovement playerMovement; // Reference to PlayerMovement script

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            AttemptToAttack();
        }

        if (animator.GetBool("IsAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.SetBool("IsAttack", false);
            playerMovement.SetIsAttacking(false);
            projectileSpawned = false;
        }
    }

    private void AttemptToAttack()
    {
        animator.SetBool("IsAttack", true);
        nextAttackTime = Time.time + attackCooldown;
        playerMovement.SetIsAttacking(true);
        projectileSpawned = false;
    }

    public void SpawnProjectileIfNeeded()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && !projectileSpawned)
        {
            ShootInDirection();
            projectileSpawned = true;
        }
    }

    void ShootInDirection()
    {
        if (magePrefab == null)
            return;

        Vector3 direction = playerMovement.facingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + (direction * projectileOffset.z) + Vector3.up * projectileOffset.y;

        GameObject mageInstance = Instantiate(magePrefab, spawnPosition, Quaternion.LookRotation(direction));
        if (mageInstance != null)
        {
            PlayerMage playerMage = mageInstance.GetComponent<PlayerMage>();
            if (playerMage != null)
            {
                playerMage.Initialize(direction);
            }
        }
    }
}
