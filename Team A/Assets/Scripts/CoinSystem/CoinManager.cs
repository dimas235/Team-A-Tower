using System.Collections;
using UnityEngine;
using TMPro; // Namespace untuk TextMeshPro

public class CoinManager : MonoBehaviour
{
    public int coins = 0; // Jumlah coin awal
    public TextMeshProUGUI coinText; // Referensi ke TextMeshProUGUI
    public float coinAddInterval = 5f; // Interval dalam detik untuk menambah coin secara otomatis
    public int coinsPerInterval = 1; // Jumlah coin yang ditambahkan setiap interval

    private void Start()
    {
        StartCoroutine(AddCoinRoutine());
        UpdateCoinUI();
    }

    private IEnumerator AddCoinRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(coinAddInterval);
            AddCoins(coinsPerInterval); // Tambahkan jumlah coin yang diatur dalam coinsPerInterval setiap interval
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = coins.ToString(); // Update UI tanpa "Coins: " karena sudah ada di canvas
        }
    }

    // Panggil metode ini saat musuh terbunuh
    public void OnEnemyKilled(int coinReward)
    {
        AddCoins(coinReward);
    }
}
