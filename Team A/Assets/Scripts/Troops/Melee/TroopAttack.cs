using UnityEngine;

public class TroopAttack : MeleeAttack
{
    protected override void PerformAttack(GameObject target)
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Physical);
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
