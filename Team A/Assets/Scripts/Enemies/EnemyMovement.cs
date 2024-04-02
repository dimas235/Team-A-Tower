using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody enemyRb;
    public float speed;



    // Update is called once per frame
    void FixedUpdate()
    {
       enemyRb.velocity = Vector2.left * speed;
    }
}
