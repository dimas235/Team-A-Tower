using UnityEngine; // Import namespace UnityEngine
using UnityEngine.UI; // Import namespace UnityEngine.UI

// Pastikan class ini tidak ditandai sebagai abstract
public class ArcherButton : DefenderButton
{    
    public GameObject archerPrefab; // Tentukan prefab di Unity Editor

    private void OnEnable()
    {
        spawnButton.onClick.AddListener(TrySpawnDefender);
    }

    private void OnDisable()
    {
        spawnButton.onClick.RemoveListener(TrySpawnDefender);
    }

    // Override metode SpawnDefender yang ditentukan di DefenderButton
    protected override void SpawnDefender()
    {
        var data = Instantiate(archerPrefab, spawnPoint.position, archerPrefab.transform.rotation);
        GameManager.instance.defenders.Add(data);
    }
}
