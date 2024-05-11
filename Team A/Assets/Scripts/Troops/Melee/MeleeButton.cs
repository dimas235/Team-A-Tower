using UnityEngine;
using UnityEngine.UI;

public class MeleeDefenderButton : DefenderButton
{
    public GameObject meleeDefenderPrefab; // Tentukan prefab untuk defender melee di Unity Editor

    private void OnEnable()
    {
        spawnButton.onClick.AddListener(TrySpawnDefender);
    }

    private void OnDisable()
    {
        spawnButton.onClick.RemoveListener(TrySpawnDefender);
    }

    protected override void SpawnDefender()
    {
        var data = Instantiate(meleeDefenderPrefab, spawnPoint.position, meleeDefenderPrefab.transform.rotation);
        GameManager.instance.defenders.Add(data);
    }
}
