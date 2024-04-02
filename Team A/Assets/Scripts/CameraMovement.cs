using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5.0f; // Kecepatan gerakan kamera

    void Update()
    {
        // Mendeteksi input dari pengguna
        if (Input.GetKey(KeyCode.D))
        {
            // Menggerakkan kamera ke kanan
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // Menggerakkan kamera ke kiri
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}
