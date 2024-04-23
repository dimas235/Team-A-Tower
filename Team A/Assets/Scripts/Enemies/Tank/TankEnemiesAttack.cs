using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemiesAttack : MeleeAttack
{
    public float stunDuration = 2.0f;

    protected override void PerformAttack(GameObject target)
    {
        DefenderHealth defenderHealth = target.GetComponent<DefenderHealth>();
        if (defenderHealth != null)
        {
            defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Physical);
            defenderHealth.ApplyStun(stunDuration);
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
}
