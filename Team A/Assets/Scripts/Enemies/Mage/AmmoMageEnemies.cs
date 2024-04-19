using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMageEnemies : MonoBehaviour
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
        ammoRb.velocity = Vector3.left * speed; // Assuming the ammo moves to the left
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TowerHealthDefens towerHealthDefens = collision.gameObject.GetComponent<TowerHealthDefens>();
        DefenderHealth defenderHealth = collision.gameObject.GetComponent<DefenderHealth>();
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); // Menambahkan ini

        if (towerHealthDefens)
        {
            towerHealthDefens.TakeDamage(damage, TowerHealthDefens.DamageType.Mage);
            Destroy(gameObject);
        }
        else if (playerHealth) // Sekarang `playerHealth` sudah didefinisikan
        {
            playerHealth.TakeDamage(damage, PlayerHealth.DamageType.Mage);
            Destroy(gameObject);
        }
        else if (defenderHealth)
        {
            defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Mage);
            Destroy(gameObject);
        }
    }
}
