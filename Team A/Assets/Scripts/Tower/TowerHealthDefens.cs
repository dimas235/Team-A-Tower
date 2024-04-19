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
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth; // Atur health ke nilai maksimum
        slider.maxValue = maxHealth; // Atur nilai maksimum slider
        slider.value = health; // Atur nilai saat ini dari slider sama dengan health
        UpdateHealthText(); // Perbarui tampilan teks untuk health
        slider.interactable = false; // Pastikan slider tidak dapat diinteraksi
    }


    public void TakeDamage(int damage, DamageType type)
    {
        health -= damage; // Kurangi health dengan damage
        health = Mathf.Clamp(health, 0, maxHealth); // Pastikan health tidak kurang dari 0 dan tidak lebih dari maxHealth
        slider.value = health; // Atur nilai slider sesuai dengan health
        UpdateHealthText(); // Perbarui tampilan teks untuk health

        // Instantiate prefab popup damage yang sesuai berdasarkan tipe damage
        GameObject selectedPrefab = type == DamageType.Mage ? popUpDamagePrefabMage : popUpDamagePrefabPhysical;
        if (selectedPrefab != null)
        {
            GameObject popup = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
            TMP_Text popupText = popup.GetComponentInChildren<TMP_Text>();
            if (popupText != null)
            {
                popupText.text = damage.ToString();
            }
        }

        if (health <= 0)
        {
            gameObject.SetActive(false); // Nonaktifkan game object
            onTowerDestroyed.Invoke(); // Panggil event onTowerDestroyed
            if (slider.gameObject != null)
            {
                Destroy(slider.gameObject); // Hancurkan game object slider
            }
        }
    }

    // public void ResetHealth()
    // {
    //     health = maxHealth;
    //     UpdateHealthText(); // Update teks saat health di-reset
    // }

    // Method untuk update teks health bar
    private void UpdateHealthText()
    {
        healthText.text = health.ToString() + "/" + maxHealth.ToString(); // Mengatur teks untuk menampilkan health
    }
}
