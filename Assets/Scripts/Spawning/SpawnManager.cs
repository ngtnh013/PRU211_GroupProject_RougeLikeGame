using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    int currentWaveIndex;
    int currentWaveSpawnCount = 0;

    public WaveData[] data;
    public Camera referenceCamera;

    [Tooltip("If there are more than this number of enemies, stop spawning any more. For performance.")]
    public int maximumEnemyCount = 300;
    float spawnTimer;
    float currentWaveDuration = 0f;

    public static SpawnManager instance;

    private void Start()
    {
        if (instance) Debug.LogWarning("There is more than 1 Spawn Manager in the Scene.");
        instance = this;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        currentWaveDuration += Time.deltaTime;

        if (spawnTimer <= 0)
        {
            if (HasWaveEnded())
            {
                currentWaveIndex++;
                currentWaveDuration = currentWaveSpawnCount = 0;

                if (currentWaveIndex >= data.Length)
                {
                    Debug.Log("All waves have been spawned! Shutting down.", this);
                    enabled = false;
                }

                return;
            }

            if (!CanSpawn())
            {
                spawnTimer += data[currentWaveIndex].GetSpawnInterval();
                return;
            }

            GameObject[] spawns = data[currentWaveIndex].GetSpawns(EnemyStats.count);

            foreach(GameObject prefab in spawns)
            {
                if (!CanSpawn()) continue;

                Instantiate(prefab, GeneratePosition(), Quaternion.identity);
                currentWaveSpawnCount++;
            }

            spawnTimer += data[currentWaveIndex].GetSpawnInterval();
        }
    }

    public bool CanSpawn()
    {
        if (HasExceededMaxEnemies()) return false;
        if (instance.currentWaveSpawnCount > instance.data[instance.currentWaveIndex].totalSpawns) return false;
        if (instance.currentWaveDuration > instance.data[instance.currentWaveIndex].duration) return false;
        return true;
    }

    public static bool HasExceededMaxEnemies()
    {
        if (!instance) return false;
        if (EnemyStats.count > instance.maximumEnemyCount) return true;
        return false;
    }

    public bool HasWaveEnded()
    {
        WaveData currentWave = data[currentWaveIndex];

        if((currentWave.exitConditions & WaveData.ExitCondition.waveDuration) > 0)
            if(currentWaveDuration < currentWave.duration) return false;

        if ((currentWave.exitConditions & WaveData.ExitCondition.reachedTotalSpawns) > 0)
            if (currentWaveSpawnCount < currentWave.totalSpawns) return false;

        if (currentWave.mustKillAll && EnemyStats.count > 0)
            return false;

        return true;
    }

    private void Reset()
    {
        referenceCamera = Camera.main;
    }

    public static Vector3 GeneratePosition()
    {
        if (!instance.referenceCamera) instance.referenceCamera = Camera.main;

        if (!instance.referenceCamera.orthographic)
            Debug.LogWarning("The reference camera is not orthographic! This will cause enemy spawns to sometime");

        float x = Random.Range(0f, 1f), y = Random.Range(0f, 1f);

        switch(Random.Range(0, 2))
        {
            case 0:
            default:
                return instance.referenceCamera.ViewportToWorldPoint(new Vector3(Mathf.Round(x), y));
            case 1:
                return instance.referenceCamera.ViewportToWorldPoint(new Vector3(x, Mathf.Round(y)));
        }
    }

    public static bool IsWithinBoundaries(Transform checkedObject)
    {
        Camera c = instance && instance.referenceCamera ? instance.referenceCamera : Camera.main;

        Vector2 viewport = c.WorldToViewportPoint(checkedObject.position);
        if (viewport.x < 0f || viewport.x > 1f) return false;
        if (viewport.y < 0f || viewport.y > 1f) return false;
        return true;
    }
}
