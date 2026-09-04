using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private float arenaMinX = -11f;
    [SerializeField] private float arenaMaxX = 11f;
    [SerializeField] private float arenaMinZ = -11f;
    [SerializeField] private float arenaMaxZ = 11f;
    [SerializeField] private int enemyCount = 5;
    [SerializeField] private int enemiesPerWave = 2;
    [SerializeField] private float minimumSpawnDistance = 6f;
    [SerializeField] private float minimumEnemySeparation = 2f;
    private int aliveEnemies;
    private List<Vector3> spawnPositions = new List<Vector3>();
    private int currentWave = 1;
    [SerializeField] private float nextWaveDelay = 2f;
    private GameUI gameUI;

    private void Start()
    {
        gameUI = FindFirstObjectByType<GameUI>();
        StartWave();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null)
        {
            Debug.LogError(
                "EnemySpawner: Enemy Prefab or Player is missing."
            );

            return;
        }

        Vector3 spawnPosition;

        int attempts = 0;

        do
        {
            spawnPosition = new Vector3(
                Random.Range(arenaMinX, arenaMaxX),
                1.25f,
                Random.Range(arenaMinZ, arenaMaxZ)
            );

            attempts++;

        } while (
            (
                Vector3.Distance(player.position, spawnPosition) <
                minimumSpawnDistance
                ||
                IsTooCloseToAnotherEnemy(spawnPosition)
            )
            && attempts < 50
        );

        spawnPositions.Add(spawnPosition);

        Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        aliveEnemies++;
    }
    private bool IsTooCloseToAnotherEnemy(Vector3 position)
    {
        foreach (Vector3 existingPosition in spawnPositions)
        {
            if (Vector3.Distance(position, existingPosition) <
                minimumEnemySeparation)
            {
                return true;
            }
        }

        return false;
    }
    public void EnemyDied()
    {
        aliveEnemies--;

        Debug.Log("Enemies Alive: " + aliveEnemies);

        if (aliveEnemies <= 0)
        {
            Invoke(nameof(SpawnNextWave), nextWaveDelay);
        }
    }
    private void SpawnNextWave()
    {
        currentWave++;

        enemyCount += enemiesPerWave;

        StartWave();
    }
    private void StartWave()
    {
        spawnPositions.Clear();

        if (gameUI != null)
        {
            gameUI.UpdateWave(currentWave);
        }   

        Debug.Log( "WAVE " + currentWave + " STARTED - Enemies: " + enemyCount);

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }
}