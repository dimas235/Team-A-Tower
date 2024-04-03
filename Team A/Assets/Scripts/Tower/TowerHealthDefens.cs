using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class TowerHealthDefens : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public UnityEvent onTowerDestroyed; // Event yang dipanggil ketika tower dihancurkan
    public Slider slider;
    public TextMeshProUGUI healthText; // Referensi untuk komponen teks (gunakan TextMeshProUGUI jika menggunakan TextMeshPro)

    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        UpdateHealthText(); // Update teks saat game dimulai
    }

    void Update()
    {
        slider.value = health;
        UpdateHealthText(); // Update teks setiap kali health berubah dalam Update

        if (health <= 0)
        {
            // Nonaktifkan tower dan panggil event onTowerDestroyed
            gameObject.SetActive(false);
            onTowerDestroyed.Invoke();
            if (slider.gameObject != null)
            {
                Destroy(slider.gameObject);
            }
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
        UpdateHealthText(); // Update teks saat health di-reset
    }

    // Method untuk update teks health bar
    private void UpdateHealthText()
    {
        healthText.text = health.ToString() + "/" + maxHealth.ToString(); // Mengatur teks untuk menampilkan health
    }
}
