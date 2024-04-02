using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    public GameObject archer;
    public Transform spawnPoint;
    public LayerMask towerLayer; // Menambahkan LayerMask untuk Tower

    public void SpawnArcher()
    {
        GameObject newArcher = Instantiate(archer, spawnPoint.position, Quaternion.identity);
        Collider[] towerColliders = Physics.OverlapSphere(spawnPoint.position, 0.1f, towerLayer);
        if (towerColliders.Length > 0)
        {
            foreach (Collider towerCollider in towerColliders)
            {
                Physics.IgnoreCollision(newArcher.GetComponent<Collider>(), towerCollider, true);
            }
            
            StartCoroutine(EnableCollision(newArcher, towerColliders));
        }
    }

    private IEnumerator EnableCollision(GameObject archer, Collider[] towerColliders)
    {
        yield return new WaitForSeconds(1f); // Tunggu selama 1 detik sebelum mengaktifkan kembali collision

        foreach (Collider towerCollider in towerColliders)
        {
            Physics.IgnoreCollision(archer.GetComponent<Collider>(), towerCollider, false);
        }
    }
}
