using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 40;
    public float attackCooldown = 0.5f;
    public float attackRange = 1.5f;
    public LayerMask targetLayer; // Lapisan untuk target yang dapat diserang
    private float lastAttackTime;

    // Dipanggil sebelum frame pertama
    void Start()
    {
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, targetLayer);
            foreach(var hit in hits)
            {
                PerformAttack(hit.gameObject);
                break; // Hanya menyerang satu target
            }
        }
    }

    private void PerformAttack(GameObject target)
    {
        TowerHealthDefens towerHealthDefens = target.GetComponent<TowerHealthDefens>();
        if (towerHealthDefens != null)
        {
            towerHealthDefens.health -= damage;
            lastAttackTime = Time.time;
            return;
        }

        DefenderHealth defenderHealth = target.GetComponent<DefenderHealth>();
        if (defenderHealth != null)
        {
            defenderHealth.health -= damage;
            lastAttackTime = Time.time;
        }
    }
}
