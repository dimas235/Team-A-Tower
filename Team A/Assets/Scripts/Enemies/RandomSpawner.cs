using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Prefab musuh untuk spawning terus menerus.
    public GameObject singleEnemyPrefab;

    // List yang berisi prefab musuh dan tingkat spawnnya.
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int spawnCount;  // Jumlah kali musuh ini muncul dalam satu siklus
    }

    public List<EnemySpawnInfo> waveEnemyInfos;

    // Waktu antara munculnya musuh secara individu.
    public float spawnTime = 2f;

    // Waktu antara munculnya wave musuh.
    public float waveSpawnTime = 10f;

    // Jumlah musuh dalam satu wave.
    public int enemiesPerWave = 5;

    // Jeda antar musuh dalam wave, dalam detik.
    public float delayBetweenWaveEnemies = 0.2f;

    // Titik dari mana musuh akan muncul.
    public Transform spawnPoint;

    private List<GameObject> enemyShuffleBag = new List<GameObject>();

    void Start()
    {
        // Mulai menghasilkan musuh secara individu.
        StartCoroutine(SpawnEnemies());

        // Mulai menghasilkan wave musuh.
        StartCoroutine(SpawnEnemyWaves());

        InitializeShuffleBag();
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnSingleEnemy();
        }
    }

    IEnumerator SpawnEnemyWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveSpawnTime);

            for (int i = 0; i < enemiesPerWave; i++)
            {
                if (enemyShuffleBag.Count == 0)
                    InitializeShuffleBag();
                
                SpawnWaveEnemy();
                yield return new WaitForSeconds(delayBetweenWaveEnemies);
            }
        }
    }

    void SpawnSingleEnemy()
    {
        Instantiate(singleEnemyPrefab, spawnPoint.position, singleEnemyPrefab.transform.rotation);
    }
    void SpawnWaveEnemy()
    {
        if (enemyShuffleBag.Count > 0)
        {
            GameObject enemyPrefab = enemyShuffleBag[0];
            enemyShuffleBag.RemoveAt(0);
            Instantiate(enemyPrefab, spawnPoint.position, enemyPrefab.transform.rotation);
        }
    }

    void InitializeShuffleBag()
    {
        enemyShuffleBag.Clear();
        foreach (var info in waveEnemyInfos)
        {
            for (int i = 0; i < info.spawnCount; i++)
            {
                enemyShuffleBag.Add(info.enemyPrefab);
            }
        }

        ShuffleBag(enemyShuffleBag);
    }

    void ShuffleBag(List<GameObject> bag)
    {
        for (int i = 0; i < bag.Count; i++)
        {
            GameObject temp = bag[i];
            int randomIndex = Random.Range(i, bag.Count);
            bag[i] = bag[randomIndex];
            bag[randomIndex] = temp;
        }
    }
}
