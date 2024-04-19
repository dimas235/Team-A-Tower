using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MageEnemiesThrowing : MonoBehaviour
{
    public GameObject ammoPrefab;
    public float coolDown;


    private float timer;
    private MageEnemiesStateMachine mageEnemiesStateMachine;

    void Start()
    {
        mageEnemiesStateMachine = GetComponent<MageEnemiesStateMachine>();
        timer = coolDown;

    }

    void Update()
    {
        if (mageEnemiesStateMachine.enemiesState != MageEnemiesStateMachine.EnemiesState.Shooting)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
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

            // Collider[] allDefenders = FindObjectsOfType<Collider>();
            // foreach (var defender in allDefenders)
            // {
            //     if (defender.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            //     {
            //         Physics.IgnoreCollision(ammoCollider, defender);
            //     }
            // }
            StartCoroutine(EnableColliderWhenPassed(ammo, ammoCollider));
            timer = coolDown;
        }
    }

    private IEnumerator EnableColliderWhenPassed(GameObject ammo, Collider ammoCollider)
    {
        yield return new WaitUntil(() => ammo.transform.position.x < transform.position.x);
        ammoCollider.enabled = true;
    }
}
