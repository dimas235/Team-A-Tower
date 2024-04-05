using UnityEngine; // Import namespace UnityEngine
using UnityEngine.UI; // Import namespace UnityEngine.UI

// Pastikan class ini tidak ditandai sebagai abstract
public class ArcherButton : DefenderButton
{
    public GameObject archerPrefab; // Tentukan prefab di Unity Editor

    // Override metode SpawnDefender yang ditentukan di DefenderButton
    protected override void SpawnDefender()
    {
        Instantiate(archerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
