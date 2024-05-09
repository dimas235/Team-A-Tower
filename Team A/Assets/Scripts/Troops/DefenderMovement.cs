using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody defenderRb;
    private bool isMovementEnabled = true;

    private DefenderHealth defenderHealth;  // Referensi ke skrip DefenderHealth

    void Start()
    {
        defenderHealth = GetComponent<DefenderHealth>();
    }

    void FixedUpdate()
    {
        if (isMovementEnabled && defenderHealth.isAlive)  // Cek apakah karakter masih hidup
        {
            defenderRb.velocity = Vector2.right * speed;
        }
        else
        {
            defenderRb.velocity = Vector2.zero;  // Hentikan pergerakan jika mati
        }
    }

    public void SetMovement(bool status)
    {
        isMovementEnabled = status;
        if (!isMovementEnabled)
        {
            defenderRb.velocity = Vector2.zero;
        }
    }
}
