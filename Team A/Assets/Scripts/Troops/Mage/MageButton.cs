using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageButton : DefenderButton
{
    public GameObject magePrefab;

    private void OnEnable() {
        spawnButton.onClick.AddListener(TrySpawnDefender);
    }

    private void OnDisable() 
    {
        spawnButton.onClick.RemoveListener(TrySpawnDefender);
    }

    protected override void SpawnDefender()
    {
       var data = Instantiate(magePrefab, spawnPoint.position, Quaternion.identity);
        GameManager.instance.defenders.Add(data);
    }

}
