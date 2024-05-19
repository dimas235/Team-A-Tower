using UnityEngine;

public class TankEnemiesAttack : MeleeAttack
{
    public float stunDuration = 2.0f;  // Default stun duration
    public float nightStunDurationIncrease = 1.0f;  // Additional stun duration during night
    private float originalStunDuration;  // Store the original stun duration to revert back
    private TimeManager timeManager;
    private bool isNightTimeBuffApplied = false;

    public bool IsAttackOnCooldown()
    {
        return Time.time - lastAttackTime < attackCooldown;
    }

    void Start()
    {
        timeManager = TimeManager.Instance;
        timeManager.OnTimeChange += HandleTimeChange;

        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            ApplyNightTimeBuff();
        }
    }

    protected override void PerformAttack(GameObject target)
    {
        if (!enabled || GetComponent<EnemyHealth>().isAlive == false)
            return;

        DefenderHealth defenderHealth = target.GetComponent<DefenderHealth>();
        if (defenderHealth != null)
        {
            defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Physical);
            lastAttackTime = Time.time;
            return;
        }

        TowerHealthDefens towerHealthDefens = target.GetComponent<TowerHealthDefens>();
        if (towerHealthDefens != null)
        {
            towerHealthDefens.TakeDamage(damage, TowerHealthDefens.DamageType.Physical);
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
            ApplyNightTimeBuff();
        }
        else
        {
            RemoveNightTimeBuff();
        }
    }

    private void ApplyNightTimeBuff()
    {
        if (!isNightTimeBuffApplied)
        {
            stunDuration += nightStunDurationIncrease;
            isNightTimeBuffApplied = true;
        }
    }

    private void RemoveNightTimeBuff()
    {
        if (isNightTimeBuffApplied)
        {
            stunDuration = originalStunDuration;
            isNightTimeBuffApplied = false;
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
