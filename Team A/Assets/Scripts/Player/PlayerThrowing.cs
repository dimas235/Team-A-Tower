using System.Collections;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    private bool projectileSpawned;
    public GameObject magePrefab;
    public float attackCooldown;
    public Vector3 projectileOffset = new Vector3(0f, 0.5f, 1.0f);  // Default offset: ke depan 1 unit, ke atas 0.5 unit

    private float lastAttackTime;
    private Animator animator;
    private PlayerMovement playerMovement; // Referensi ke skrip PlayerMovement

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // Inisialisasi playerMovement
    }

    void Update()
    {
        lastAttackTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && lastAttackTime <= 0 && !animator.GetBool("IsAttack"))
        {
            animator.SetBool("IsAttack", true);
            lastAttackTime = attackCooldown;
            playerMovement.SetIsAttacking(true);
            projectileSpawned = false; // Reset the projectile spawn flag
        }

        // Make sure to reset IsAttack to allow for re-entry into the Attack state
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && animator.GetBool("IsAttack"))
        {
            animator.SetBool("IsAttack", false);
            playerMovement.SetIsAttacking(false);
            projectileSpawned = false; // Reset the projectile spawn flag
        }
    }

    public void SpawnProjectileIfNeeded()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && !projectileSpawned)
        {
            ShootInDirection();
            projectileSpawned = true; // Set the flag to true after spawning the projectile
        }
    }

    void ShootInDirection()
    {
        if (magePrefab == null)
        {
            return;  // Jika magePrefab belum diatur, tidak melakukan apa-apa
        }

        Vector3 direction = playerMovement.facingRight ? Vector3.right : Vector3.left; // Menyesuaikan arah berdasarkan orientasi karakter
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
