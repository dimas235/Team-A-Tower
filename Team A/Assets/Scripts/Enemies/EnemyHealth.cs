using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefabPhysical; // Prefab untuk damage fisik
    public GameObject popUpDamagePrefabMage;     // Prefab untuk damage mage
    public int coinValue = 5; // Jumlah coin yang diberikan saat musuh terbunuh

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage, DamageType type)
    {
        health -= damage;
        
        // Menentukan prefab berdasarkan jenis damage
        GameObject selectedPrefab = type == DamageType.Mage ? popUpDamagePrefabMage : popUpDamagePrefabPhysical;
        if (selectedPrefab != null)
        {
            GameObject popup = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
            TMP_Text popupText = popup.GetComponentInChildren<TMP_Text>();
            if (popupText != null)
            {
                popupText.text = damage.ToString();
            }
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        CoinManager coinManager = FindObjectOfType<CoinManager>();
        if (coinManager != null)
        {
            coinManager.OnEnemyKilled(coinValue);
        }

        Destroy(gameObject);
    }
}
