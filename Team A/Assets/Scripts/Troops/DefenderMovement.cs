using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody defenderRb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (enabled)
        {
            defenderRb.velocity = Vector2.right * speed;
        }
        else
        {
            defenderRb.velocity = Vector2.zero; // Menghentikan Rigidbody ketika DefenderMovement dinonaktifkan
        }
    }
}
