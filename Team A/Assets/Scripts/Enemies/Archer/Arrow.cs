using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody arrowRb;
    public float speed;
    public float range;
    public int damage;
    public LayerMask ArrowDefenderLayer;

    public float nonCollisionRadius = 1f;

    private float timer;

    void Start()
    {
        timer = range;
    }

    void FixedUpdate()
    {
        arrowRb.velocity = Vector2.left * speed;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TowerHealthDefens towerHealthDefens = collision.gameObject.GetComponent<TowerHealthDefens>();
        DefenderHealth defenderHealth = collision.gameObject.GetComponent<DefenderHealth>();
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); // Menambahkan ini

        Collider[] hits = Physics.OverlapSphere(transform.position, nonCollisionRadius, ArrowDefenderLayer);
        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), hit, true);
            }
        }

        if (towerHealthDefens)
        {
            towerHealthDefens.takeDamage(damage);
            Destroy(gameObject);
        }
        else if (playerHealth) // Sekarang `playerHealth` sudah didefinisikan
        {
            playerHealth.takeDamage(damage);
            Destroy(gameObject);
        }
        else if (defenderHealth)
        {
            defenderHealth.takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
