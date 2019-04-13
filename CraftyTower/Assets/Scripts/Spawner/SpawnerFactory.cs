using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CraftyTower.Spawner;

public static class SpawnerFactory {

    public static ISpawner CreateSpawner(SpawnerSettings settings)
    {
        switch (settings.spawnMode)
        {
            case SpawnMode.Single:
                return new SimpleSpawner(settings.simpleSpawnerSettings);
            case SpawnMode.Wave:
                return new WaveSpawner(settings.waveSpawnerSettings);
            case SpawnMode.TimedWave:
                return new TimedWaveSpawner(settings.timedSpawnerSettings);
        }
        return null;
    }
}
