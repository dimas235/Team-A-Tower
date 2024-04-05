using UnityEngine;
using UnityEngine.UI;

public class MeleeDefenderButton : DefenderButton
{
    public GameObject meleeDefenderPrefab; // Tentukan prefab untuk defender melee di Unity Editor

    protected override void SpawnDefender()
    {
        Instantiate(meleeDefenderPrefab, spawnPoint.position, Quaternion.identity);
    }
}
