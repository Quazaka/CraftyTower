using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner : ISpawner
{
    SpawnerSettings.SimpleSpawnerSettings settings;

    public SimpleSpawner(SpawnerSettings.SimpleSpawnerSettings settings)
    {
        this.settings = settings;
        EnemiesSpawned = settings.enemiesSpawned;
        EnemiesToSpawn = settings.enemiesToSpawn;
        TimeToNextSpawn = settings.timeSinceLastSpawn;
    }

    public float TimeToNextSpawn { get; set; }
    public int EnemiesToSpawn { get; set ; }
    public int EnemiesSpawned { get; set; }
    public bool Spawn { get; set; }

    public void DoSpawn(SpawnDelegate createEnemy)
    {
        if (Spawn)
        {
            if (EnemiesSpawned >= EnemiesToSpawn)
            {
                EnemiesSpawned = 0;
                Spawn = false;
            }
            else
            {
                TimeToNextSpawn += Time.deltaTime;
                while (TimeToNextSpawn >= settings.spawnRate)
                {
                    TimeToNextSpawn -= settings.spawnRate;
                    EnemiesSpawned++;
                    createEnemy();
                }
            }
        }
    }
}
