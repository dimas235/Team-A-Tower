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
    public GameObject popUpDamagePrefab;
    public TMP_Text popUpText; 



    void Start()
    {
        health = maxHealth; // Atur health ke nilai maksimum
        slider.maxValue = maxHealth; // Atur nilai maksimum slider
        slider.value = health; // Atur nilai saat ini dari slider sama dengan health
        UpdateHealthText(); // Perbarui tampilan teks untuk health
        slider.interactable = false; // Pastikan slider tidak dapat diinteraksi
    }


    public void takeDamage(int damage)
    {
        health -= damage; // Kurangi health sesuai dengan damage yang diterima
        health = Mathf.Clamp(health, 0, maxHealth); // Pastikan health tidak kurang dari 0 dan tidak lebih dari maxHealth
        slider.value = health; // Update nilai slider setelah mengubah health
        UpdateHealthText(); // Update teks health

        popUpText.text = damage.ToString();
        Instantiate(popUpDamagePrefab, transform.position, Quaternion.identity);

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
