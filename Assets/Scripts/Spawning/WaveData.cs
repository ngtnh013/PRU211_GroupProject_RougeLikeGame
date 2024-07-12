using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "2D Top-down Rouge-like/Wave Data")]
public class WaveData : SpawnData
{
    [Header("Wave Data")]

    [Tooltip("If there are less than this number of enemies, we will keep spawning until we get there.")]
    [Min(1)] public int startingCount = 0;

    [Tooltip("How many enemies can this wave spawn at maximum?")]
    [Min(1)] public uint totalSpawns = uint.MaxValue;

    [System.Flags] public enum ExitCondition { waveDuration = 1, reachedTotalSpawns = 2}
    [Tooltip("Set the things that can trigger the end of this wave.")]
    public ExitCondition exitConditions = (ExitCondition)1;

    [Tooltip("All enemies must be dead for the wave to advance.")]
    public bool mustKillAll = false;

    [HideInInspector] public uint spawnCount;

    public override GameObject[] GetSpawns(int totalEnemies = 0)
    {
        int count = Random.Range(spawnsPerTick.x, spawnsPerTick.y);

        if(totalEnemies + count < startingCount)
            count = startingCount - totalEnemies;

        GameObject[] result = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = possibleSpawnPrefabs[Random.Range(0, possibleSpawnPrefabs.Length)];
        }

        return result;
    }
}
