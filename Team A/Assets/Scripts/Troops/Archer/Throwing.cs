using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject stonePrefab;
    public float coolDown;
    public float detectionRange;
    public LayerMask enemyLayer;
    public Vector3 spawnOffset;
    private float timer = 0f;
    private Animator animator;
    private bool firstAttackDone = false;
    private DefenderHealth defenderHealth;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        defenderHealth = GetComponentInParent<DefenderHealth>();
        // Mengatur timer ke durasi cooldown tetapi mengizinkan serangan pertama
        timer = 0f; 
    }

    void Update()
    {
        if (defenderHealth != null && !defenderHealth.isAlive)
            return;
        if (animator.GetBool("IsStunned"))
            return;

        if (firstAttackDone && timer > 0)
            timer -= Time.deltaTime;

        if (timer <= 0 && CheckForEnemy())
        {
            LaunchStone();
            firstAttackDone = true;
            timer = coolDown;
            animator.SetBool("IsCooldown", true); // Pastikan cooldown diaktifkan
            Invoke("EndCooldown", coolDown);
        }
    }

    private void LaunchStone()
    {
        // Menyusun posisi spawn berdasarkan offset dari objek ini
        Vector3 spawnPosition = transform.position + spawnOffset;
        Instantiate(stonePrefab, spawnPosition, transform.rotation);
        animator.SetBool("IsAttack", true); // Mengatur animator untuk menyerang
    }

    private void EndCooldown()
    {
        animator.SetBool("IsCooldown", false); // Mengakhiri cooldown
    }

    private bool CheckForEnemy()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.right, out hit, detectionRange, enemyLayer);
    }
}
