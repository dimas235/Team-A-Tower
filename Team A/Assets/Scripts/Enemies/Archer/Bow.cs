using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float coolDown;
    public float detectionRange;
    public LayerMask defenderLayer;
    public Vector3 spawnOffset;  // Offset posisi spawn dari posisi objek ini

    private float timer = 0f;
    private Animator animator;
    private bool firstAttackDone = false;
    private EnemyHealth enemyHealth;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
        timer = 0f;
    }

    void Update()
    {
        if (enemyHealth != null && !enemyHealth.isAlive)
            return;
        // if (animator.GetBool("IsStunned"))
        //     return;

        if (firstAttackDone && timer > 0)
            timer -= Time.deltaTime;

        if (timer <= 0 && CheckForDefender())
        {
            LaunchArrow();
            firstAttackDone = true;
            timer = coolDown;
            animator.SetBool("IsCooldown", true);
            Invoke("EndCooldown", coolDown);
        }
    }

    private void LaunchArrow()
    {
        // Instantiate arrow at the calculated position relative to this object
        Vector3 spawnPosition = transform.position + spawnOffset;
        Instantiate(arrowPrefab, spawnPosition, transform.rotation);
        animator.SetBool("IsAttack", true);
    }

    private void EndCooldown()
    {
        animator.SetBool("IsCooldown", false);
    }

    private bool CheckForDefender()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.left, out hit, detectionRange, defenderLayer);
    }
}
