using UnityEngine;

public abstract class MeleeAttack : MonoBehaviour
{
    public int damage = 40;
    public float attackCooldown = 0.5f;
    public float attackRange = 1.5f;
    public LayerMask targetLayer;
    protected float lastAttackTime = -1;

    protected virtual void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown && (!GetComponent<EnemyHealth>() || !GetComponent<EnemyHealth>().isStunned))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, targetLayer);
            foreach (var hit in hits)
            {
                PerformAttack(hit.gameObject);
                break; // Hanya menyerang satu target
            }
        }
    }

    protected abstract void PerformAttack(GameObject target);
}