using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan gerak karakter
    private Rigidbody rigidbody; // Komponen Rigidbody untuk pengendalian fisika
    public bool facingRight = true; // Arah menghadap karakter
    private Animator animator; // Komponen Animator
    private bool isAttacking = false; // Flag untuk menandai apakah karakter sedang menyerang
    private PlayerHealth playerHealth; // Referensi ke PlayerHealth

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); // Mengambil komponen Rigidbody
        animator = GetComponent<Animator>(); // Mengambil komponen Animator
        playerHealth = GetComponent<PlayerHealth>(); // Mengambil komponen PlayerHealth
    }

    void FixedUpdate()
    {
        if (isAttacking || playerHealth.isDead) // Jika karakter sedang menyerang atau mati, tidak memproses gerakan
        {
            return;
        }

        float movementInput = 0f;
        if (Input.GetKey(KeyCode.D)) {
            movementInput = 1f;
        } else if (Input.GetKey(KeyCode.A)) {
            movementInput = -1f;
        }

        animator.SetBool("IsRunning", Mathf.Abs(movementInput) > 0); // Mengaktifkan atau menonaktifkan animasi berlari

        Vector3 move = new Vector3(movementInput * moveSpeed, rigidbody.velocity.y, 0f);
        rigidbody.velocity = move; // Menerapkan pergerakan berdasarkan input dan kecepatan

        // Flip the character based on input direction
        if ((movementInput > 0 && !facingRight) || (movementInput < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 rotation = transform.eulerAngles;
        rotation.y += 180f; // Tambah 180 derajat ke sumbu Y
        transform.eulerAngles = rotation; // Terapkan rotasi
    }

    public void SetIsAttacking(bool attacking)
    {
        isAttacking = attacking;
        animator.SetBool("IsAttack", attacking); // Mengatur status serangan pada animator
    }
}
