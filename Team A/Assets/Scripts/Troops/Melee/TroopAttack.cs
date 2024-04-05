using UnityEngine;

public class TroopAttack : MeleeAttack
{
    protected override void PerformAttack(GameObject target)
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.takeDamage(damage);
            lastAttackTime = Time.time;
            return;
        }

        TowerHealthAttacker towerHealthAttacker = target.GetComponent<TowerHealthAttacker>();
        if (towerHealthAttacker != null)
        {
            towerHealthAttacker.takeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}
