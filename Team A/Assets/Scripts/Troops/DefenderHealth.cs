using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DefenderHealth : MonoBehaviour
{

    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefab;
    public TMP_Text popUpText; 


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
            Destroy(gameObject);
        }
    }


}
