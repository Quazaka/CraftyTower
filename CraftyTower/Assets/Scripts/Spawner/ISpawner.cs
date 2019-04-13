public delegate void SpawnDelegate();

public interface ISpawner
{
    float TimeToNextSpawn { get; set; }
    int EnemiesToSpawn { get; set; }
    int EnemiesSpawned { get; set; }
    bool Spawn { get; set; }

    void DoSpawn(SpawnDelegate createEnemy);
}
