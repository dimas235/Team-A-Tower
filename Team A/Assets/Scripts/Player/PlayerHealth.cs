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
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;

    public bool isStunned = false;
    public float stunDuration = 0;
    public event System.Action OnStunEnded;

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

    public void ApplyStun(float duration)
    {
        if (duration > 0 && !isStunned)
        {
            isStunned = true;
            stunDuration = duration;
            StartCoroutine(StunCountdown(duration));
        }
        else if (duration == 0)
        {
            isStunned = false;
        }
    }

    private IEnumerator StunCountdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStunned = false;
        OnStunEnded?.Invoke();
    }

    // public void ResetHealth()
    // {
    //     health = maxHealth;
    //     UpdateHealthText();
    // }

    private void Die()
    {
        // ResetHealth();
        // gameObject.SetActive(false);
        // onPlayerDestroyed.Invoke();
        // if (slider.gameObject != null)
        // {
        //     Destroy(slider.gameObject);
        // }
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }
}