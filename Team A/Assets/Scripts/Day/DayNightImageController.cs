using UnityEngine;
using UnityEngine.UI; // Namespace yang diperlukan untuk bekerja dengan UI

public class DayNightImageController : MonoBehaviour
{
    [SerializeField] private Image afternoonImage;  // Drag gambar afternoon ke sini melalui Unity Editor
    [SerializeField] private Image nightImage;      // Drag gambar night ke sini melalui Unity Editor

    [Header("Transparency Settings")]
    public float dayTransparency = 1f;      // Transparansi untuk siang hari
    public float nightTransparency = 0.5f;  // Transparansi untuk malam hari

    void Start()
    {
        TimeManager.Instance.OnTimeChange += UpdateImageTransparency;
        UpdateImageTransparency();  // Memastikan transparansi diatur benar pada awal game
    }

    void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeChange -= UpdateImageTransparency;  // Bersihkan event saat objek dihancurkan
        }
    }

    private void UpdateImageTransparency()
    {
        if (TimeManager.Instance.currentTimeOfDay == TimeManager.TimeOfDay.Day)
        {
            SetTransparency(afternoonImage, dayTransparency);  // Penuh visible
            SetTransparency(nightImage, nightTransparency);    // Setengah transparan
        }
        else
        {
            SetTransparency(afternoonImage, nightTransparency); // Setengah transparan
            SetTransparency(nightImage, dayTransparency);       // Penuh visible
        }
    }

    private void SetTransparency(Image image, float alpha)
    {
        Color newColor = image.color;
        newColor.a = alpha;
        image.color = newColor;
    }
}
