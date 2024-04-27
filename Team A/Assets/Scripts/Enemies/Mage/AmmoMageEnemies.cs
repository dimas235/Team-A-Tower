using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMageEnemies : MonoBehaviour
{

    public Rigidbody ammoRb;
    public float speed;
    public float range;
    public int damage;
    public int maxHits = 3;  // Jumlah maksimal hit sebelum proyektil hancur

    private float timer;
    private int hitCount;  // Menghitung berapa kali proyektil telah mengenai musuh

    void Start()
    {
        timer = range;
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
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

    // private void OnCollisionEnter(Collision collision)
    // {
    //     TowerHealthDefens towerHealthDefens = collision.gameObject.GetComponent<TowerHealthDefens>();
    //     DefenderHealth defenderHealth = collision.gameObject.GetComponent<DefenderHealth>();
    //     PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); // Menambahkan ini

    //     if (towerHealthDefens)
    //     {
    //         towerHealthDefens.TakeDamage(damage, TowerHealthDefens.DamageType.Mage);
    //         Destroy(gameObject);
    //     }
    //     else if (playerHealth) // Sekarang `playerHealth` sudah didefinisikan
    //     {
    //         playerHealth.TakeDamage(damage, PlayerHealth.DamageType.Mage);
    //         Destroy(gameObject);
    //     }
    //     else if (defenderHealth)
    //     {
    //         defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Mage);
    //         Destroy(gameObject);
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        TowerHealthDefens towerHealthDefens = other.GetComponent<TowerHealthDefens>();
        DefenderHealth defenderHealth = other.GetComponent<DefenderHealth>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>(); // Menambahkan ini


        if (towerHealthDefens != null)
        {
            towerHealthDefens.TakeDamage(damage, TowerHealthDefens.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
        else if (defenderHealth != null)
        {
            defenderHealth.TakeDamage(damage, DefenderHealth.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
        else if (playerHealth != null) // Sekarang `playerHealth` sudah didefinisikan
        {
            playerHealth.TakeDamage(damage, PlayerHealth.DamageType.Mage);
            hitCount++;
            CheckForDestruction();
        }
    }

    private void CheckForDestruction()
    {
        if (hitCount >= maxHits)
        {
            Destroy(gameObject);
        }
    }

    
}
