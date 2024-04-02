using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderHealth : MonoBehaviour
{

    public int health;
    public int maxHealth = 100;
    void Start()
    {
        health = maxHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
