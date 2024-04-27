using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Array untuk menampung prefab musuh.
    public GameObject[] enemyPrefabs;

    // Waktu antara munculnya musuh.
    public float spawnTime = 2f;

    // Titik dari mana musuh akan muncul.
    public Transform spawnPoint;

    void Start()
    {
        // Mulai menghasilkan musuh.
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Menunggu waktu yang ditentukan dalam spawnTime
            yield return new WaitForSeconds(spawnTime);

            // Memilih prefab musuh secara acak.
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Membuat instansiasi musuh acak di titik muncul.
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}