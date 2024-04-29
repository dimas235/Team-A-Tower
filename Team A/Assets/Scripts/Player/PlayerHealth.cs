using System.Collections;
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

    public float regenRate = 1;          // Kesehatan yang diregen per detik.
    public float regenDelay = 5;         // Waktu delay dalam detik sebelum regenerasi dimulai.
    private Coroutine regenCoroutine;    // Coroutine untuk regenerasi kesehatan.
    private float lastDamageTime;        // Waktu terakhir pemain terkena serangan.

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
        lastDamageTime = Time.time;
    }

    public void TakeDamage(int damage, DamageType type)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        slider.value = health;
        UpdateHealthText();
        lastDamageTime = Time.time;

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
        else
        {
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }
            regenCoroutine = StartCoroutine(DelayRegenHealth());
        }
    }

    private IEnumerator DelayRegenHealth()
    {
        yield return new WaitForSeconds(regenDelay);

        while (health < maxHealth && (Time.time - lastDamageTime >= regenDelay))
        {
            health += Mathf.FloorToInt(regenRate);
            health = Mathf.Clamp(health, 0, maxHealth);
            slider.value = health;
            UpdateHealthText();
            yield return new WaitForSeconds(1);
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

    private void Die()
    {
        
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }
}
