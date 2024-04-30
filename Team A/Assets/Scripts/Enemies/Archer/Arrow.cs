using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody arrowRb;
    public float speed;
    public float range;
    public int damage = 10; // Default damage
    public int nightDamageBonus = 20; // Public additional damage for night

    private float timer;
    private bool isNightTimeDamageBuffApplied = false;
    private TimeManager timeManager;
    private int originalDamage; // Store original damage to revert back

    void Start()
    {
        timer = range;
        timeManager = TimeManager.Instance;
        timeManager.OnTimeChange += HandleTimeChange;
        originalDamage = damage; // Store the original damage

        // Apply damage buff if it starts during the night
        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            ApplyNightTimeDamageBuff();
        }
    }

    void FixedUpdate()
    {
        arrowRb.velocity = Vector2.left * speed;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
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
            damage = originalDamage + nightDamageBonus; // Add flat bonus
            isNightTimeDamageBuffApplied = true;
        }
    }

    private void RemoveNightTimeDamageBuff()
    {
        if (isNightTimeDamageBuffApplied)
        {
            damage = originalDamage; // Revert to original damage
            isNightTimeDamageBuffApplied = false;
        }
    }

    private void OnDestroy()
    {
        if (timeManager != null)
        {
            timeManager.OnTimeChange -= HandleTimeChange;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TowerHealthDefens towerHealthDefens = collision.gameObject.GetComponent<TowerHealthDefens>();
        DefenderHealth defenderHealth = collision.gameObject.GetComponent<DefenderHealth>();
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (towerHealthDefens)
        {
            towerHealthDefens.TakeDamage(damage, TowerHealthDefens.DamageType.Physical);
            Destroy(gameObject);
        }
        else if (playerHealth)
        {
            playerHealth.TakeDamage(damage, PlayerHealth.DamageType.Physical);
            Destroy(gameObject);
        }
        else if (defenderHealth)
        {
            defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Physical);
            Destroy(gameObject);
        }
    }
}
