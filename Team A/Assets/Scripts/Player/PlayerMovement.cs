using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan pergerakan karakter
    private CharacterController controller; // Komponen CharacterController untuk mengendalikan gerakan
    private float movementInput; // Input dari pemain untuk gerakan
    public bool facingRight = true; // Status apakah karakter menghadap ke kanan
    private Animator animator; // Komponen Animator untuk mengendalikan animasi
    private bool isAttacking = false; // Status apakah karakter sedang menyerang

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Mengambil komponen CharacterController
        animator = GetComponent<Animator>(); // Mengambil komponen Animator
    }

    void Update()
    {
        // Jika karakter sedang menyerang, keluar dari metode Update
        if (isAttacking)
        {
            return;
        }

        movementInput = 0f; // Reset input gerakan
        if (Input.GetKey(KeyCode.D))
        {
            movementInput = 1f; // Gerakan ke kanan
            animator.SetBool("IsRunning", true); // Mengaktifkan animasi berlari
            if (!facingRight) // Jika sedang menghadap kiri, flip ke kanan
            {
                Flip();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementInput = -1f; // Gerakan ke kiri
            animator.SetBool("IsRunning", true); // Mengaktifkan animasi berlari
            if (facingRight) // Jika sedang menghadap kanan, flip ke kiri
            {
                Flip();
            }
        }
        else
        {
            animator.SetBool("IsRunning", false); // Menonaktifkan animasi berlari
        }

        // Menerapkan pergerakan berdasarkan input dan kecepatan
        Vector3 move = new Vector3(movementInput * moveSpeed * Time.deltaTime, 0f, 0f);
        controller.Move(move);
    }

    // Fungsi untuk membalik arah karakter
    void Flip()
    {
        facingRight = !facingRight; // Membalikkan status menghadap
        transform.rotation = Quaternion.Euler(0, facingRight ? 90 : -90, 0); // Menggunakan operator ternary untuk menentukan sudut
    }

    // Metode untuk mengatur status serangan
    public void SetIsAttacking(bool attacking)
    {
        isAttacking = attacking; // Menetapkan status serangan
        animator.SetBool("IsAttack", attacking); // Mengatur animasi serangan
    }
}
