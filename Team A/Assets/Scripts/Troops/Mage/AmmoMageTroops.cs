using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMageTroops : MonoBehaviour
{
    public Rigidbody ammoRb;
    public float speed;
    public float range;
    public int damage;

    private float timer;

    void Start()
    {
        timer = range;
    }

    void FixedUpdate()
    {
        ammoRb.velocity = Vector3.right * speed; // Assuming the ammo moves to the right
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        TowerHealthAttacker towerHealthAttacker = collision.gameObject.GetComponent<TowerHealthAttacker>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Mage);
            Destroy(gameObject);
        }
        else if (towerHealthAttacker != null)
        {
            towerHealthAttacker.TakeDamage(damage, TowerHealthAttacker.DamageType.Mage);
            Destroy(gameObject);
        }
    }
}
