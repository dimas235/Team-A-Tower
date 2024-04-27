using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankButton : DefenderButton

{
    public GameObject tankPrefab; // Tentukan prefab untuk tank di Unity Editor

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
        var data = Instantiate(tankPrefab, transform.position, Quaternion.identity);
        GameManager.instance.defenders.Add(data);
    }
}
