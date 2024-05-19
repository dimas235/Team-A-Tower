using UnityEngine;

public class EnemyBuffManager : MonoBehaviour
{
    public int healthBuffAmount = 50; // Jumlah kesehatan tambahan saat malam

    private EnemyHealth enemyHealth;
    private TimeManager timeManager;
    private int lastMaxHealth; // Menyimpan nilai maxHealth sebelum buff diterapkan

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        timeManager = TimeManager.Instance;
        timeManager.OnTimeChange += ApplyNightBuff;

        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            ApplyNightBuff();
        }
    }

    private void ApplyNightBuff()
    {
        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            lastMaxHealth = enemyHealth.maxHealth;
            enemyHealth.maxHealth += healthBuffAmount;
            int healthIncrease = enemyHealth.maxHealth - lastMaxHealth;
            enemyHealth.health += healthIncrease;
        }
        else
        {
            int healthDecrease = enemyHealth.maxHealth - lastMaxHealth;
            enemyHealth.health -= healthDecrease;
            enemyHealth.maxHealth -= healthBuffAmount;
        }
    }

    private void OnDestroy()
    {
        timeManager.OnTimeChange -= ApplyNightBuff;
    }
}
