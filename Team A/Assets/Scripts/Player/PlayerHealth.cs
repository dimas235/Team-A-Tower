using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Image healthImage;
    public TextMeshProUGUI healthText;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public Animator animator;

    public Material blinkMaterial; // Referensi ke material blink
    public float flashDuration = 0.1f; // Durasi flash
    private Renderer[] renderers; // Renderer untuk objek
    private Material[] originalMaterials; // Menyimpan material asli

    public float regenRate = 1;
    public float regenDelay = 5;
    private Coroutine regenCoroutine;
    private float lastDamageTime;
    public bool isDead = false;

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

        // Inisialisasi renderers dan originalMaterials
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
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

        // Panggil efek flash saat terkena damage
        StartCoroutine(FlashEffect());

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

    private IEnumerator FlashEffect()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = blinkMaterial;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
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
