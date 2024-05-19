using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody enemyRb;
    public float speed;
    private bool canMove = true;

    private EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void FixedUpdate()
    {
        if (canMove && enemyHealth.isAlive)
        {
            enemyRb.velocity = Vector2.left * speed;
        }
        else
        {
            enemyRb.velocity = Vector2.zero;
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
