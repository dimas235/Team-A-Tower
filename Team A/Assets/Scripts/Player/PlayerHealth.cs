using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Image healthImage; // Change from Slider to Image
    public TextMeshProUGUI healthText;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public Animator animator;

    public float regenRate = 1;
    public float regenDelay = 5;
    private Coroutine regenCoroutine;
    private float lastDamageTime;
    public bool isDead = false;  // Public untuk diakses oleh PlayerMovement

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
        lastDamageTime = Time.time;
    }

    void Update()
    {
        if (isDead)
        {
            // Gravitasi bisa diaplikasikan di sini jika menggunakan Rigidbody
        }
    }

    public void TakeDamage(int damage, DamageType type)
    {
        if (health <= 0 || isDead) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
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

    private void Die()
    {
        if (!isDead)
        {
            animator.SetTrigger("IsDeath");
            isDead = true;
            gameObject.layer = LayerMask.NameToLayer("IgnoreProjectiles");
            this.enabled = false;  // Nonaktifkan skrip ini
        }
    }

    private IEnumerator DelayRegenHealth()
    {
        yield return new WaitForSeconds(regenDelay);
        while (health < maxHealth && (Time.time - lastDamageTime >= regenDelay) && !isDead)
        {
            health += Mathf.FloorToInt(regenRate);
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthUI();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateHealthUI()
    {
        float fillAmount = (float)health / maxHealth;
        healthImage.fillAmount = fillAmount;
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }
}
