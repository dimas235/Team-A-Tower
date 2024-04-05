using UnityEngine;

public class EnemyAttack : MeleeAttack
{
    protected override void PerformAttack(GameObject target)
    {
        TowerHealthDefens towerHealthDefens = target.GetComponent<TowerHealthDefens>();
        if (towerHealthDefens != null)
        {
            towerHealthDefens.takeDamage(damage);
            lastAttackTime = Time.time;
            return;
        }

        DefenderHealth defenderHealth = target.GetComponent<DefenderHealth>();
        if (defenderHealth != null)
        {
            defenderHealth.takeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}
