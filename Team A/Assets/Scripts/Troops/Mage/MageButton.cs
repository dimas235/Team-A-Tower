using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageButton : DefenderButton
{
    public GameObject magePrefab;

    protected override void SpawnDefender()
    {
        Instantiate(magePrefab, spawnPoint.position, Quaternion.identity);
    }

}
