using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public int coinReward = 10; // Jumlah koin dasar yang diberikan saat musuh mati
    public float nightTimeRewardMultiplier = 1.5f; // Pengali rewards saat malam hari, sekarang sebagai float
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public Animator animator;
    public bool isAlive = true;

    public CoinManager coinManager; // Referensi ke CoinManager
    public TimeManager timeManager; // Referensi ke TimeManager

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        coinManager = FindObjectOfType<CoinManager>(); // Temukan instance CoinManager
        timeManager = TimeManager.Instance; // Mendapatkan instance TimeManager
    }

    public void TakeDamage(int damage, DamageType type)
    {
        if (!isAlive)
            return;

        health -= damage;

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

    private void Die()
    {
        animator.SetTrigger("Death");
        isAlive = false;
        gameObject.layer = LayerMask.NameToLayer("IgnoreProjectiles");
        DisableOtherComponents();
        Destroy(gameObject, 5.0f);

        if (coinManager != null)
        {
            // Hitung jumlah koin yang harus diberikan
            int coinsToGive = Mathf.RoundToInt(coinReward * (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night ? nightTimeRewardMultiplier : 1f));
            coinManager.AddCoins(coinsToGive); // Tambahkan koin sesuai dengan pengali
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
