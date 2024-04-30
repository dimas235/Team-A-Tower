using UnityEngine;

public class EnemyBuffManager : MonoBehaviour
{
    public int healthBuffAmount = 50; // Jumlah kesehatan tambahan saat malam

    private EnemyHealth enemyHealth;
    private TimeManager timeManager;
    private int lastMaxHealth; // Menyimpan nilai maxHealth sebelum buff diterapkan

    void Start()
    {
        // Mendapatkan referensi komponen EnemyHealth pada musuh
        enemyHealth = GetComponent<EnemyHealth>();

        // Mendapatkan referensi TimeManager dari singleton Instance
        timeManager = TimeManager.Instance;

        // Subscribe ke event OnTimeChange untuk menerapkan buff
        timeManager.OnTimeChange += ApplyNightBuff;

        // Memastikan buff diterapkan pada awal permainan jika saat ini adalah malam
        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            ApplyNightBuff();
        }
    }

    private void ApplyNightBuff()
    {
        // Memeriksa apakah saat ini malam hari
        if (timeManager.currentTimeOfDay == TimeManager.TimeOfDay.Night)
        {
            // Simpan maxHealth sebelum menerapkan buff
            lastMaxHealth = enemyHealth.maxHealth;

            // Menambahkan kesehatan tambahan pada musuh
            enemyHealth.maxHealth += healthBuffAmount;

            // Hitung peningkatan relatif berdasarkan perbedaan aktual antara health dan maxHealth
            int healthIncrease = enemyHealth.maxHealth - lastMaxHealth;

            // Tambahkan kesehatan sebanyak peningkatan relatif
            enemyHealth.health += healthIncrease;
        }
        else
        {
            // Mengembalikan ke nilai awal jika tidak lagi malam hari
            int healthDecrease = enemyHealth.maxHealth - lastMaxHealth;
            enemyHealth.health -= healthDecrease;
            enemyHealth.maxHealth -= healthBuffAmount;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe dari event OnTimeChange saat obyek dihancurkan
        timeManager.OnTimeChange -= ApplyNightBuff;
    }
}
