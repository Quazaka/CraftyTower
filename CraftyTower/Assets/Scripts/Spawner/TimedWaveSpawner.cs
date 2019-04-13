using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedWaveSpawner : ISpawner {

    SpawnerSettings.TimedSpawnerSettings settings;

    public TimedWaveSpawner(SpawnerSettings.TimedSpawnerSettings settings)
    {
        this.settings = settings;
        EnemiesSpawned = settings.enemiesSpawned;
        EnemiesToSpawn = settings.enemiesToSpawn;
        TimeToNextSpawn = settings.timeSinceLastSpawn;
    }

    public float TimeToNextSpawn { get; set; }
    public int EnemiesToSpawn { get; set; }
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
