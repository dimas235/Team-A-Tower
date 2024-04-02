using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    // public float delayTime = .15f; // Variabel ini tidak lagi diperlukan
    // public EnemyMovement enemyMovement; // Variabel ini tidak lagi diperlukan jika kita tidak menonaktifkan gerakan untuk knockback

    void Start()
    {
        health = maxHealth;   
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        // StartCoroutine(knockbackDelay()); // Menghilangkan panggilan ke coroutine knockbackDelay

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Coroutine knockbackDelay tidak lagi diperlukan dan bisa dihapus
    /*
    IEnumerator knockbackDelay()
    {
        enemyMovement.enabled = false;
        yield return new WaitForSeconds(delayTime);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            enemyMovement.enabled = true;
        }
    }
    */
}
