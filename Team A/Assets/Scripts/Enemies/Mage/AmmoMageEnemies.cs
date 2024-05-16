using System.Collections;
using UnityEngine;

public class AmmoMageEnemies : MonoBehaviour
{
    public Rigidbody ammoRb;
    public float speed;
    public float range;
    public int damage;
    public int nightDamageBonus = 20;  // Additional damage during night
    public int maxHits = 3;  // Maximum number of hits before the projectile destroys
    public GameObject fireballParticlesPrefab; // Reference to the fireball particles prefab

    private float timer;
    private int hitCount;  // Count how many times the projectile has hit enemies
    private TimeManager timeManager;
    private int originalDamage; // Store the original damage to revert back
    private bool isNightTimeDamageBuffApplied = false;
    private GameObject fireballParticlesInstance;

    void Start()
    {
        timer = range;
        originalDamage = damage; // Store the original damage
        timeManager = TimeManager.Instance;
        timeManager.OnTimeChange += HandleTimeChange;

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        // Apply damage buff if it starts during the night
        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            ApplyNightTimeDamageBuff();
        }

        // Instantiate fireball particles and attach to the projectile
        fireballParticlesInstance = Instantiate(fireballParticlesPrefab, transform.position, Quaternion.identity);
        fireballParticlesInstance.transform.parent = transform;
    }

    void FixedUpdate()
    {
        ammoRb.velocity = Vector3.left * speed;  // Assuming the ammo moves to the left
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TowerHealthDefens towerHealthDefens = other.GetComponent<TowerHealthDefens>();
        DefenderHealth defenderHealth = other.GetComponent<DefenderHealth>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (towerHealthDefens != null)
        {
            towerHealthDefens.TakeDamage(damage, TowerHealthDefens.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
        else if (defenderHealth != null)
        {
            defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
        else if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage, PlayerHealth.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
    }

    private void HandleTimeChange()
    {
        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            ApplyNightTimeDamageBuff();
        }
        else
        {
            RemoveNightTimeDamageBuff();
        }
    }

    private void ApplyNightTimeDamageBuff()
    {
        if (!isNightTimeDamageBuffApplied)
        {
            damage += nightDamageBonus;  // Add flat bonus
            isNightTimeDamageBuffApplied = true;
        }
    }

    private void RemoveNightTimeDamageBuff()
    {
        if (isNightTimeDamageBuffApplied)
        {
            damage -= nightDamageBonus;  // Subtract the added bonus
            isNightTimeDamageBuffApplied = false;
        }
    }

    private void CheckForDestruction()
    {
        if (hitCount >= maxHits)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (timeManager != null)
        {
            timeManager.OnTimeChange -= HandleTimeChange;
        }

        // Ensure to destroy the particle effect when the projectile is destroyed
        if (fireballParticlesInstance != null)
        {
            Destroy(fireballParticlesInstance);
        }
    }
}
