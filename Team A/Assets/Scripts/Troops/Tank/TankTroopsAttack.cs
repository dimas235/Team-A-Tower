using UnityEngine;

public class TankTroopsAttack : MeleeAttack
{
    public float stunDuration = 2.0f;

    public bool IsAttackOnCooldown()
    {
        return Time.time - lastAttackTime < attackCooldown;
    }

    protected override void PerformAttack(GameObject target)
    {
        if (!enabled || GetComponent<DefenderHealth>().isAlive == false)
            return;

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
