using UnityEngine;

public class Stone : MonoBehaviour
{
    public Rigidbody stoneRb;
    public float speed;
    public float range;
    public int damage;
    // public float knockbackForce; // Variabel ini tidak diperlukan lagi karena knockback dihilangkan

    private float timer;

    void Start()
    {
        timer = range;
    }

    void FixedUpdate()
    {
        stoneRb.velocity = Vector2.right * speed;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        TowerHealthAttacker towerHealthAttacker = collision.gameObject.GetComponent<TowerHealthAttacker>();

        if(enemyHealth)
        {
            enemyHealth.takeDamage(damage);
            Destroy(gameObject);
        }
        else if (towerHealthAttacker)
        {
            towerHealthAttacker.takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
