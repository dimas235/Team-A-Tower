using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float attackCooldown;

    private float lastAttackTime;
    private EnemyStateMachineArcher stateMachine;

    void Start()
    {
        stateMachine = GetComponent<EnemyStateMachineArcher>();
        lastAttackTime = attackCooldown;
    }

    void Update()
    {
        if(stateMachine.enemiesState != EnemyStateMachineArcher.EnemiesState.Shooting)
        {
            return;
        }

        lastAttackTime -= Time.deltaTime;
        if(lastAttackTime <= 0)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            Collider arrowCollider = arrow.GetComponent<Collider>();
            if(arrowCollider != null)
            {
                arrowCollider.enabled = false;
            }

            Collider parentCollider = GetComponentInParent<Collider>();
            if(parentCollider != null)
            {
                Physics.IgnoreCollision(arrowCollider, parentCollider);
            }

            Collider[] allEnemies = FindObjectsOfType<Collider>();
            foreach (var enemy in allEnemies)
            {
                if(enemy.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    Physics.IgnoreCollision(arrowCollider, enemy);
                }
            }

            StartCoroutine(EnableColliderWhenPassed(arrow, arrowCollider));
            lastAttackTime = attackCooldown;
        }
    }

    private IEnumerator EnableColliderWhenPassed(GameObject arrow, Collider arrowCollider)
    {
        yield return new WaitUntil(() => arrow.transform.position.x < transform.position.x);
        arrowCollider.enabled = true;
    }
}
