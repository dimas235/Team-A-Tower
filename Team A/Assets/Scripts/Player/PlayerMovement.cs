using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isMoving;
    
    private Rigidbody rb;
    private float movementInput;
    public bool facingRight = true; // Dijadikan public untuk diakses oleh PlayerThrowing

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isMoving = false;

        movementInput = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            movementInput = 1f;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementInput = -1f;
            isMoving = true;
        }

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
        rb.MovePosition(rb.position + new Vector3(movementInput, 0f, 0f) * moveSpeed * Time.fixedDeltaTime);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
