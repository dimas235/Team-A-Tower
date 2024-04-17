using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Referensi ke Transform pemain
    public Vector3 offset; // Offset posisi kamera dari pemain
    public float cameraSpeed = 5f; // Kecepatan gerakan kamera saat digeser dengan mouse
    public float returnSpeed = 10f; // Kecepatan kamera kembali fokus ke pemain

    private bool isFollowingPlayer = true; // Apakah kamera mengikuti pemain
    private Vector3 lastPlayerPosition; // Untuk menyimpan posisi pemain dari frame terakhir

    void Start()
    {
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        // Periksa input mouse kanan untuk menggeser kamera
        if (Input.GetMouseButton(1))
        {
            isFollowingPlayer = false; // Stop mengikuti pemain saat menggeser kamera
            float mouseX = Input.GetAxis("Mouse X") * cameraSpeed;
            transform.Translate(mouseX, 0, 0, Space.World);
        }

        // Periksa apakah pemain bergerak dengan membandingkan posisi sekarang dengan terakhir
        if (player.position != lastPlayerPosition)
        {
            isFollowingPlayer = true; // Mulai mengikuti pemain lagi
            lastPlayerPosition = player.position; // Perbarui posisi terakhir pemain
        }
    }

    void LateUpdate()
    {
        // Jika kamera mengikuti pemain, perbarui posisi kamera berdasarkan pemain
        if (isFollowingPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * returnSpeed);
        }
    }
}
