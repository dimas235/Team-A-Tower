using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemiesThrowing : MonoBehaviour
{
    public GameObject ammoPrefab;
    public float coolDown;

    private float timer;
    private MageEnemiesStateMachine mageEnemiesStateMachine;
    private EnemyHealth enemyHealth;  // Referensi ke EnemyHealth

    void Start()
    {
        mageEnemiesStateMachine = GetComponent<MageEnemiesStateMachine>();
        enemyHealth = GetComponent<EnemyHealth>();  // Setel referensi EnemyHealth
        timer = coolDown;
    }

    void Update()
    {
        if (mageEnemiesStateMachine.enemiesState != MageEnemiesStateMachine.EnemiesState.Shooting || enemyHealth.isStunned)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            FireAmmo();
            timer = coolDown;
        }
    }

    private void FireAmmo()
    {
        GameObject ammo = Instantiate(ammoPrefab, transform.position, Quaternion.identity);
        Collider ammoCollider = ammo.GetComponent<Collider>();
        if (ammoCollider != null)
        {
            ammoCollider.enabled = false;
        }

        Collider parentCollider = GetComponentInParent<Collider>();
        if (parentCollider != null)
        {
            Physics.IgnoreCollision(ammoCollider, parentCollider);
        }

        StartCoroutine(EnableColliderWhenPassed(ammo, ammoCollider));
    }

    private IEnumerator EnableColliderWhenPassed(GameObject ammo, Collider ammoCollider)
    {
        yield return new WaitUntil(() => ammo.transform.position.x < transform.position.x);
        ammoCollider.enabled = true;
    }
}
