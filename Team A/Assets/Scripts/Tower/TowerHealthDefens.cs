using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TowerHealthDefens : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public UnityEvent onTowerDestroyed; // Event yang dipanggil ketika tower dihancurkan
    public Image healthImage; // Ganti dari Slider ke Image
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
        UpdateHealthUI(); // Perbarui tampilan UI untuk health
    }

    public void TakeDamage(int damage, DamageType type)
    {
        health -= damage; // Kurangi health dengan damage
        health = Mathf.Clamp(health, 0, maxHealth); // Pastikan health tidak kurang dari 0 dan tidak lebih dari maxHealth
        UpdateHealthUI(); // Perbarui tampilan UI untuk health

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
        }
    }

    // Method untuk update UI health
    private void UpdateHealthUI()
    {
        if (healthImage != null)
        {
            healthImage.fillAmount = (float)health / maxHealth; // Mengatur fill amount dari Image berdasarkan health
        }
        if (healthText != null)
        {
            healthText.text = health.ToString() + "/" + maxHealth.ToString(); // Mengatur teks untuk menampilkan health
        }
    }
}
