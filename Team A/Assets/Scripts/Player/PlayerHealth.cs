using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Slider slider;
    public TextMeshProUGUI healthText;
    public GameObject popUpDamagePrefab;
    public TMP_Text popUpText;

    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
        UpdateHealthText();
        slider.interactable = false;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        slider.value = health;
        UpdateHealthText();

        popUpText.text = damage.ToString();
        Instantiate(popUpDamagePrefab, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }
}
