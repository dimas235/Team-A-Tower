using UnityEngine;

public class Stone : MonoBehaviour
{
    public Rigidbody stoneRb;
    public float speed;
    public float range;
    public int damage;

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

        if (enemyHealth != null)
        {
            // Menggunakan versi yang benar dari TakeDamage dengan enum DamageType
            enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Physical);
            Destroy(gameObject);
        }
        else if (towerHealthAttacker != null)
        {
            // Panggil dengan huruf 'T' besar sesuai dengan konvensi
            towerHealthAttacker.TakeDamage(damage, TowerHealthAttacker.DamageType.Physical);
            Destroy(gameObject);
        }
    }
}
