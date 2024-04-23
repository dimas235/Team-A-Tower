using System.Collections;
using UnityEngine;

public class TankTroopsAttack : MeleeAttack
{
    public float stunDuration = 2.0f;

    protected override void PerformAttack(GameObject target)
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Physical);
            enemyHealth.ApplyStun(stunDuration);
            lastAttackTime = Time.time;
            return;
        }

        TowerHealthAttacker towerHealthAttacker = target.GetComponent<TowerHealthAttacker>();
        if (towerHealthAttacker != null)
        {
            towerHealthAttacker.TakeDamage(damage, TowerHealthAttacker.DamageType.Physical);
            lastAttackTime = Time.time;
        }
    }
}
