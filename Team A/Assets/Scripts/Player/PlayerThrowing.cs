using System.Collections;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    public GameObject magePrefab;
    public Transform staffTransform;
    public float attackCooldown;
    public Vector3 projectileOffset = new Vector3(0f, 0.5f, 1.0f);  // Default offset: ke depan 1 unit, ke atas 0.5 unit

    private float lastAttackTime;
    private Animator animator;
    private PlayerMovement playerMovement; // Referensi ke skrip PlayerMovement

    void Start()
    {
        animator = GetComponent<Animator>();
        lastAttackTime = attackCooldown;
        playerMovement = GetComponent<PlayerMovement>(); // Inisialisasi playerMovement
    }

    void Update()
    {
        lastAttackTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && lastAttackTime <= 0 && !animator.GetBool("IsAttacking"))
        {
            animator.SetBool("IsAttack", true); // Memulai animasi serangan
            lastAttackTime = attackCooldown;
            playerMovement.SetIsAttacking(true); // Set isAttacking menjadi true
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            animator.SetBool("IsAttack", false); // Menghentikan animasi serangan
            playerMovement.SetIsAttacking(false); // Set isAttacking menjadi false
        }
    }

    void ShootInDirection()
    {
        if (magePrefab == null)
        {
            Debug.LogError("Mage prefab has not been assigned in the Inspector");
            return;
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
            else
            {
                Debug.LogError("PlayerMage component not found on the magePrefab");
            }
        }
        else
        {
            Debug.LogError("Failed to instantiate magePrefab");
        }
    }
}
