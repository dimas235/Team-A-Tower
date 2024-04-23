using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float attackCooldown;

    private float lastAttackTime;
    private EnemyHealth enemyHealth;  // Referensi ke EnemyHealth

    void Start()
    {
        lastAttackTime = attackCooldown;
        enemyHealth = GetComponentInParent<EnemyHealth>();  // Ambil referensi dari parent
    }

    void Update()
    {
        if (!enabled || enemyHealth.isStunned)
        {
            return;  // Jangan lakukan apapun jika Bow tidak aktif atau musuh ter-stun
        }

        lastAttackTime -= Time.deltaTime;
        if (lastAttackTime <= 0)
        {
            ShootArrow();
            lastAttackTime = attackCooldown;
        }
    }

    private void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Collider arrowCollider = arrow.GetComponent<Collider>();
        if (arrowCollider != null)
        {
            arrowCollider.enabled = false;
        }

        Collider parentCollider = GetComponentInParent<Collider>();
        if (parentCollider != null)
        {
            Physics.IgnoreCollision(arrowCollider, parentCollider);
        }

        StartCoroutine(EnableColliderWhenPassed(arrow, arrowCollider));
    }

    private IEnumerator EnableColliderWhenPassed(GameObject arrow, Collider arrowCollider)
    {
        yield return new WaitUntil(() => arrow.transform.position.x < transform.position.x);
        arrowCollider.enabled = true;
    }
}
