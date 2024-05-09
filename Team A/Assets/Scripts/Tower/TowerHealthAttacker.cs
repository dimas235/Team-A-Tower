using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TowerHealthAttacker : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public UnityEvent onTowerDestroyed;
    public Slider slider;
    public TextMeshProUGUI healthText;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;

    // Add isAlive property
    public bool isAlive = true;

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
        UpdateHealthText();
        slider.interactable = false;
    }

    public void TakeDamage(int damage, DamageType type)
    {
        if (!isAlive) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        slider.value = health;
        UpdateHealthText();

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
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        isAlive = false;  // Set isAlive to false upon destruction
        onTowerDestroyed.Invoke();
        if (slider.gameObject != null)
        {
            Destroy(slider.gameObject);
        }
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }
}
