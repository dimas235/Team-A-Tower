using UnityEngine;
using DG.Tweening;

public class CloudsController : MonoBehaviour
{
    public Transform[] clouds; // Array untuk menyimpan semua objek awan
    public float durationMin = 2f; // Durasi minimal animasi
    public float durationMax = 5f; // Durasi maksimal animasi
    public float moveAmount = 50f; // Jarak gerakan ke kanan dan ke kiri dari posisi awal

    void Start()
    {
        foreach (var cloud in clouds)
        {
            StartAnimation(cloud);
        }
    }

    void StartAnimation(Transform cloud)
    {
        float duration = Random.Range(durationMin, durationMax);

        // Tween yang bergerak ke kanan dan kembali ke posisi awal secara otomatis
        cloud.DOMoveX(cloud.position.x + moveAmount, duration)
             .SetEase(Ease.InOutSine) // Menggunakan easing untuk gerakan yang halus
             .SetLoops(-1, LoopType.Yoyo); // Membuat animasi berulang bolak-balik tanpa jeda
    }
}
