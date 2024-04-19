using UnityEngine;

public class EnemyAttack : MeleeAttack
{
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
}
