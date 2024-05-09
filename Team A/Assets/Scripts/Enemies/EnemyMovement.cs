using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody enemyRb;
    public float speed;
    private bool canMove = true;

    private EnemyHealth enemyHealth;  // Referensi ke skrip EnemyHealth

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }


    void FixedUpdate()
    {
            if (canMove && enemyHealth.isAlive)  // Cek apakah karakter masih hidup
        {
            enemyRb.velocity = Vector2.left * speed;
        }
        else
        {
            enemyRb.velocity = Vector2.zero;  // Hentikan pergerakan jika mati
        }
    }

    public void SetMovement(bool status)
    {
        canMove = status;
        if (!canMove)
        {
            enemyRb.velocity = Vector2.zero;
        }
    }
}
