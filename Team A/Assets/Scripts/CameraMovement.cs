using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Referensi ke Transform pemain
    public Vector3 offset; // Offset posisi kamera dari pemain
    public float edgeBoundary = 50f; // Jarak dari tepi layar untuk menggerakkan kamera
    public float cameraSpeed = 5f; // Kecepatan gerakan kamera

    private bool focusOnPlayer = true;

    void Update()
    {
        // Cek jika kursor berada dekat dengan tepi layar dan fokus kamera tidak sedang di pemain
        if (!focusOnPlayer)
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.mousePosition.x >= Screen.width - edgeBoundary)
            {
                moveDirection.x += 1f;
            }
            else if (Input.mousePosition.x <= edgeBoundary)
            {
                moveDirection.x -= 1f;
            }

            transform.Translate(moveDirection * cameraSpeed * Time.deltaTime, Space.World);
        }
    }

    void LateUpdate()
    {
        // Fokus ke pemain jika player bergerak atau jika fokus kamera di set ke player
        if (player != null && (focusOnPlayer || PlayerMoved()))
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * cameraSpeed);
        }
    }

    // Fungsi untuk mengecek apakah pemain telah bergerak
    bool PlayerMoved()
    {
        // Cek perubahan posisi player disini, bisa menggunakan threshold jika diperlukan
        // Sebagai contoh sederhana, kita hanya cek jika player tidak di posisi yang sama dengan posisi kamera (dengan offset)
        return (player.position + offset) != transform.position;
    }

    // Panggil fungsi ini untuk mengatur fokus kamera ke player, misalnya setelah bergerak atau melakukan aksi tertentu
    public void FocusOnPlayer()
    {
        focusOnPlayer = true;
    }

    // Panggil fungsi ini untuk membebaskan kamera dari fokus player
    public void FreeCamera()
    {
        focusOnPlayer = false;
    }
}
