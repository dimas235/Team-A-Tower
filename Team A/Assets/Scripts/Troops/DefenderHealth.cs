using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DefenderHealth : MonoBehaviour
{

    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;


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
        Destroy(gameObject);
    }
}
