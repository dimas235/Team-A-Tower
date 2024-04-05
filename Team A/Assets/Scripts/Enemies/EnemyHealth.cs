using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefab;
    public TMP_Text popUpText; 
    public int coinValue = 5; // Jumlah coin yang diberikan saat musuh terbunuh

    void Start()
    {
        health = maxHealth;   
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        popUpText.text = damage.ToString();
        Instantiate(popUpDamagePrefab, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            // Cari instance dari CoinManager dan tambahkan coin
            CoinManager coinManager = FindObjectOfType<CoinManager>();
            if (coinManager != null)
            {
                coinManager.OnEnemyKilled(coinValue);
            }

            Destroy(gameObject);
        }
    }
}
