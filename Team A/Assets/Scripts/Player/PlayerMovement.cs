using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;
    private float movementInput;
    public bool facingRight = true; 
    private Animator animator; 
    private bool isAttacking = false; // Menandai apakah karakter sedang menyerang

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        // Jika karakter sedang menyerang, keluar dari metode Update
        if (isAttacking)
        {
            return;
        }

        movementInput = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            movementInput = 1f;
            animator.SetBool("IsRunning", true); 
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementInput = -1f;
            animator.SetBool("IsRunning", true); 
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        Vector3 move = new Vector3(movementInput * moveSpeed * Time.deltaTime, 0f, 0f);
        controller.Move(move);

        if (movementInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (movementInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    // Metode untuk mengatur status serangan
    public void SetIsAttacking(bool attacking)
    {
        isAttacking = attacking;
    }
}
