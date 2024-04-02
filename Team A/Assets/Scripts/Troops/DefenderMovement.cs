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

    // Update is called once per frame
    void FixedUpdate()
    {
         defenderRb.velocity = Vector2.right * speed;
        
    }
}
