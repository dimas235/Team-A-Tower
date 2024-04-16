using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody rb;
    private float movementInput;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Mendapatkan input dari keyboard dengan KeyCode
        movementInput = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            movementInput = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementInput = -1f;
        }
        
        // Memutar karakter jika bergerak ke kiri atau kanan
        if (movementInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (movementInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        // Bergerak setiap frame fisika
        rb.MovePosition(rb.position + new Vector3(movementInput, 0f, 0f) * moveSpeed * Time.fixedDeltaTime);
    }

    void Flip()
    {
        // Memutar karakter
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // Mengganti sisi dari karakter
        transform.localScale = theScale;
    }
}
