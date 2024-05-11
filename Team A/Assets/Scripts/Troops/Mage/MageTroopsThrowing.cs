using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTroopsThrowing : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float coolDown;
    public float detectionRange;
     public LayerMask enemyLayer;
    public Vector3 spawnOffset;
    private float timer = 0f;
    private Animator animator;
    private bool firstAttackDone = false;
    private DefenderHealth defenderHealth;  // Referensi ke kesehatan


    void Start()
    {
        animator = GetComponentInParent<Animator>();
        defenderHealth = GetComponent<DefenderHealth>();
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
            LaunchFireball();
            firstAttackDone = true;
            timer = coolDown;
            animator.SetBool("IsCooldown", true); // Pastikan cooldown diaktifkan
            Invoke("EndCooldown", coolDown);
        }
    }

    private void LaunchFireball()
    {
        // Menyusun posisi spawn berdasarkan offset dari objek ini
        Vector3 spawnPosition = transform.position + spawnOffset;
        Instantiate(fireballPrefab, spawnPosition, transform.rotation);
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
