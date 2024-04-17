using UnityEngine;

public class PlayerMage : MonoBehaviour
{
    public Rigidbody mageRb;
    public float speed;
    public float range;
    public int damage;
    public LayerMask enemyLayer;

    private float timer;

    void Start()
    {
        timer = range;
    }

    public void Initialize(Vector3 direction)
    {
        mageRb.velocity = direction * speed;
    }

    void FixedUpdate()
    {
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        timer -= Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Panggil metode takeDamage dengan huruf 't' kecil sesuai definisi di kelas EnemyHealth
                enemyHealth.takeDamage(damage); 
            }
            Destroy(gameObject);
        }
    }
}
