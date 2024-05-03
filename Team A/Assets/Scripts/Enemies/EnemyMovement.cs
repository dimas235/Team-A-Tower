using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody enemyRb;
    public float speed;
    private bool canMove = true;

    // Memperbarui apakah enemy bisa bergerak atau tidak
    public void SetMovement(bool status)
    {
        canMove = status;
        if (!canMove)
        {
            enemyRb.velocity = Vector2.zero;  // Menghentikan musuh
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            enemyRb.velocity = Vector2.left * speed;
        }
    }
}
