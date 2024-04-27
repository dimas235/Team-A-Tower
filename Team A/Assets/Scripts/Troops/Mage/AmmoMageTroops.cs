using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMageTroops : MonoBehaviour
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
        timer = range;  // Timer untuk autodestruksi berdasarkan jangkauan
        // Pastikan collider proyektil adalah trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    void FixedUpdate()
    {
        ammoRb.velocity = Vector3.right * speed; // Asumsi proyektil bergerak ke kanan
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);  // Hancurkan proyektil jika telah mencapai batas jangkauan
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Memeriksa jika objek yang bertabrakan adalah musuh atau menara
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        TowerHealthAttacker towerHealthAttacker = other.GetComponent<TowerHealthAttacker>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Mage);
            hitCount++;  // Meningkatkan jumlah hit
            CheckForDestruction();
        }
        else if (towerHealthAttacker != null)
        {
            towerHealthAttacker.TakeDamage(damage, TowerHealthAttacker.DamageType.Mage);
            hitCount++;  // Meningkatkan jumlah hit
            CheckForDestruction();
        }
    }

    // Memeriksa apakah jumlah hit telah mencapai batas maksimum
    private void CheckForDestruction()
    {
        if (hitCount >= maxHits)
        {
            Destroy(gameObject);  // Hancurkan proyektil jika telah mencapai jumlah maksimum hit
        }
    }
}
