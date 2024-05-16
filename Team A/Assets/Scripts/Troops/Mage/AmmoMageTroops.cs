using UnityEngine;

public class AmmoMageTroops : MonoBehaviour
{
    public Rigidbody ammoRb;
    public float speed;
    public float range;
    public int damage;
    public int maxHits = 3;  // Maximum hits before the projectile is destroyed
    public GameObject fireballParticlesPrefab; // Reference to the fireball particles prefab

    private float timer;
    private int hitCount;  // Count of how many times the projectile has hit enemies
    private GameObject fireballParticlesInstance;

    void Start()
    {
        timer = range;  // Timer for auto-destruction based on range

        // Instantiate fireball particles and attach to the projectile
        fireballParticlesInstance = Instantiate(fireballParticlesPrefab, transform.position, Quaternion.identity);
        fireballParticlesInstance.transform.parent = transform;

        // Ensure the projectile's collider is a trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    void FixedUpdate()
    {
        ammoRb.velocity = Vector3.right * speed; // Assuming projectile moves to the right
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);  // Destroy projectile if it has reached its range limit
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit: " + other.name); // For debugging to see what the projectile hits
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        TowerHealthAttacker towerHealthAttacker = other.GetComponent<TowerHealthAttacker>();

        if (enemyHealth != null)
        {
            Debug.Log("Damaging enemy");
            enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
        else if (towerHealthAttacker != null)
        {
            Debug.Log("Damaging tower");
            towerHealthAttacker.TakeDamage(damage, TowerHealthAttacker.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
    }

    // Check if the projectile has reached the maximum number of hits
    private void CheckForDestruction()
    {
        if (hitCount >= maxHits)
        {
            Destroy(gameObject);  // Destroy the projectile if it has reached the maximum number of hits
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
