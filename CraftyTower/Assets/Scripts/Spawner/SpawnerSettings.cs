using UnityEngine;
using CraftyTower.Spawner;

[CreateAssetMenu]
public class SpawnerSettings : ScriptableObject {

    public SpawnMode spawnMode;

    public SimpleSpawnerSettings simpleSpawnerSettings;
    public WaveSpawnerSettings waveSpawnerSettings;
    public TimedSpawnerSettings timedSpawnerSettings;

    [System.Serializable]
    public class SimpleSpawnerSettings
    {
        [Range(0.1f, 5)]
        public float spawnRate = 1f;
        [HideInInspector]
        public float timeSinceLastSpawn;
        public int enemiesToSpawn;
        public int enemiesSpawned;
    }

    [System.Serializable]
    public class WaveSpawnerSettings : SimpleSpawnerSettings
    {
        [Range(1, 10)]
        public int waitBetweenWaves = 5;
    }

    [System.Serializable]
    public class TimedSpawnerSettings : SimpleSpawnerSettings
    {
        [Range(20, 60)]
        public int waveTime = 20;
    }
}
