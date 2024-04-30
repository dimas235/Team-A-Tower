using UnityEngine;

public class EnemyAttack : MeleeAttack
{
    public int nightDamageBonus = 20;  // Additional damage during night
    private TimeManager timeManager;
    private int originalDamage; // Store the original damage to revert back
    private bool isNightTimeDamageBuffApplied = false;

    void Start()
    {
        timeManager = TimeManager.Instance;
        timeManager.OnTimeChange += HandleTimeChange;
        originalDamage = damage; // Store the original damage

        // Apply damage buff if it starts during the night
        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            ApplyNightTimeDamageBuff();
        }
    }

    protected override void PerformAttack(GameObject target)
    {
        TowerHealthDefens towerHealthDefens = target.GetComponent<TowerHealthDefens>();
        if (towerHealthDefens != null)
        {
            towerHealthDefens.TakeDamage(damage, TowerHealthDefens.DamageType.Physical);
            lastAttackTime = Time.time;
            return;
        }

        DefenderHealth defenderHealth = target.GetComponent<DefenderHealth>();
        if (defenderHealth != null)
        {
            defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Physical);
            lastAttackTime = Time.time;
        }

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage, PlayerHealth.DamageType.Physical);
            lastAttackTime = Time.time;
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
            damage = originalDamage;  // Revert to original damage
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
}
