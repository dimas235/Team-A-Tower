using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public int coinReward = 10;
    public float nightTimeRewardMultiplier = 1.5f;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public Animator animator;
    public bool isAlive = true;

    public CoinManager coinManager;
    public TimeManager timeManager;

    public Material blinkMaterial; // Referensi ke material blink
    public float flashDuration = 0.1f; // Durasi flash
    private Renderer[] renderers; // Renderer untuk objek
    private Material[] originalMaterials; // Menyimpan material asli

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        coinManager = FindObjectOfType<CoinManager>();
        timeManager = TimeManager.Instance;

        // Inisialisasi renderers dan originalMaterials
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    public void TakeDamage(int damage, DamageType type)
    {
        if (!isAlive)
            return;

        health -= damage;

        // Panggil efek flash saat terkena damage
        StartCoroutine(FlashEffect());

        if (health <= 0)
        {
            Die();
            return;
        }

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
        animator.SetTrigger("Death");
        isAlive = false;
        gameObject.layer = LayerMask.NameToLayer("IgnoreProjectiles");
        DisableOtherComponents();
        Destroy(gameObject, 5.0f);

        if (coinManager != null)
        {
            int coinsToGive = Mathf.RoundToInt(coinReward * (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night ? nightTimeRewardMultiplier : 1f));
            coinManager.AddCoins(coinsToGive);
        }
    }

    private void DisableOtherComponents()
    {
        var movementComponent = GetComponent<EnemyMovement>();
        if (movementComponent != null)
            movementComponent.SetMovement(false);

        var attackComponent = GetComponent<TankEnemiesAttack>();
        if (attackComponent != null)
            attackComponent.enabled = false;
    }
}
