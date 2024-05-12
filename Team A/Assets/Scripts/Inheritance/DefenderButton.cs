using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;  // Tambahkan baris ini


public abstract class DefenderButton : MonoBehaviour
{
    public Button spawnButton;
    public Image cooldownOverlay;
    public CoinManager coinManager;
    public Transform spawnPoint;

    public float spawnCooldown;
    public int spawnCost;
    
    protected bool isCooldown = false;

    // Keycode untuk aktivasi tombol
    public KeyCode activationKey;

    protected abstract void SpawnDefender();

    protected virtual void Awake()
    {
        cooldownOverlay.fillAmount = 0;
        // Tambahkan listener untuk klik mouse
        spawnButton.onClick.AddListener(TrySpawnDefender);
    }

    protected virtual void Update()
    {
        spawnButton.interactable = coinManager.coins >= spawnCost && !isCooldown;

        // Cek jika tombol keyboard ditekan
        if (Input.GetKeyDown(activationKey) && spawnButton.interactable)
        {
            TrySpawnDefender();
        }
    }

    public void TrySpawnDefender()
    {
        if (coinManager.coins >= spawnCost && !isCooldown)
        {
            SpawnDefender();
            coinManager.AddCoins(-spawnCost);
            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        cooldownOverlay.gameObject.SetActive(true);

        for (float i = 0; i < spawnCooldown; i += Time.deltaTime)
        {
            cooldownOverlay.fillAmount = i / spawnCooldown;
            yield return null;
        }

        cooldownOverlay.fillAmount = 0;
        cooldownOverlay.gameObject.SetActive(false);
        isCooldown = false;
    }
}
