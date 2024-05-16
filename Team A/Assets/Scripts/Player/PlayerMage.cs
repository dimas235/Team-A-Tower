using UnityEngine;

public class PlayerMage : MonoBehaviour
{
    public Rigidbody mageRb;
    public float speed;
    public float range;
    public int damage;
    public LayerMask enemyLayer;
    public GameObject fireballParticlesPrefab; // Referensi ke prefab partikel fireball

    private float timer;
    private GameObject fireballParticlesInstance;

    void Start()
    {
        timer = range;

        // Instantiate fireball particles
        fireballParticlesInstance = Instantiate(fireballParticlesPrefab, transform.position, Quaternion.identity);
        fireballParticlesInstance.transform.parent = transform; // Attach to the projectile
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
                enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Mage);
            }
            Destroy(gameObject);
        }

        TowerHealthAttacker towerHealthAttacker = collision.gameObject.GetComponent<TowerHealthAttacker>();

        if (towerHealthAttacker != null)
        {
            towerHealthAttacker.TakeDamage(damage, TowerHealthAttacker.DamageType.Mage);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Ensure to destroy the particle effect when the fireball is destroyed
        if (fireballParticlesInstance != null)
        {
            Destroy(fireballParticlesInstance);
        }
    }
}
