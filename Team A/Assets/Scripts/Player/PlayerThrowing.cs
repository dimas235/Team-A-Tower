using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrowing : MonoBehaviour
{
    private bool projectileSpawned;
    public GameObject magePrefab;
    public GameObject meteorPrefab; // Prefab untuk skill attack
    public float attackCooldown = 1.0f;  // Cooldown serangan biasa
    public float skillCooldown = 5.0f;  // Cooldown serangan skill (public)
    public Vector3 projectileOffset = new Vector3(0f, 0.5f, 1.0f);  // Offset default untuk serangan biasa
    public Vector3 skillProjectileOffset = new Vector3(0f, 10f, 0f);  // Offset default untuk skill attack

    private float nextAttackTime = 0f;
    private float nextSkillTime = 0f;
    private Animator animator;
    private PlayerMovement playerMovement; // Referensi ke skrip PlayerMovement

    public LayerMask enemyLayer; // Layer untuk musuh
    public ProjectileCooldownUI projectileCooldownUI; // Referensi ke skrip UI cooldown

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

        if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextSkillTime)
        {
            AttemptToSkillAttack();
        }

        if (animator.GetBool("IsAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.SetBool("IsAttack", false);
            playerMovement.SetIsAttacking(false);
            projectileSpawned = false;
        }

        if (animator.GetBool("IsSkillAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.SetBool("IsSkillAttack", false);
            playerMovement.SetIsAttacking(false);
            projectileSpawned = false;
        }

        // Update the skillCooldown parameter in the Animator
        animator.SetFloat("skillCooldown", Mathf.Max(0, nextSkillTime - Time.time));
    }

    private void AttemptToAttack()
    {
        animator.SetBool("IsAttack", true);
        animator.SetBool("IsSkillAttack", false);
        nextAttackTime = Time.time + attackCooldown;
        playerMovement.SetIsAttacking(true);
        projectileSpawned = false;
    }

    private void AttemptToSkillAttack()
    {
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsSkillAttack", true);
        nextSkillTime = Time.time + skillCooldown;
        playerMovement.SetIsAttacking(true);
        projectileSpawned = false;
    }

    private GameObject FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100f, enemyLayer);
        GameObject nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = collider.gameObject;
            }
        }

        return nearestEnemy;
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

    void SpawnSkillProjectile()
    {
        if (meteorPrefab == null)
            return;

        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null)
            return;

        Vector3 spawnPosition = transform.position + new Vector3(skillProjectileOffset.x, skillProjectileOffset.y, 0f);

        GameObject meteorInstance = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
        if (meteorInstance != null)
        {
            SkillProjectile skillProjectile = meteorInstance.GetComponent<SkillProjectile>();
            if (skillProjectile != null)
            {
                skillProjectile.Initialize(nearestEnemy.transform.position);
                projectileCooldownUI.StartCooldown(); // Mulai cooldown ketika proyektil berhasil di-spawn
            }
        }
    }
}
