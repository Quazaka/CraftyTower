using System.Collections.Generic;
using UnityEngine;
using CraftyTower.Spawner;

public class SpawnController : MonoBehaviour {

    public delegate void SpawnDelegate();
    
    public SpawnerSettings settings;
    private ISpawner spawnerType;
    private SpawnMode spawnMode;
    [SerializeField] SpawnZone spawnZone;

    [HideInInspector]
    public bool spawnerSettingsFoldout;

    [SerializeField]
    EnemyFactory enemyFactory;
    List<Enemy> enemies;

    private void Awake()
    {
        enemies = new List<Enemy>();
        spawnerType = SpawnerFactory.CreateSpawner(settings);
        spawnMode = settings.spawnMode;
    }

    private void Update()
    {
        if (spawnMode != settings.spawnMode)
        {
            Debug.Log("spawnmode changed from " + spawnMode + " to: " + settings.spawnMode);
            Debug.Log(spawnerType.EnemiesToSpawn);
            spawnMode = settings.spawnMode;
            spawnerType = SpawnerFactory.CreateSpawner(settings);
            Debug.Log(spawnerType.EnemiesToSpawn);
        }
    }

    public void Spawn()
    {
        spawnerType.DoSpawn(CreateEnemy);
    }

    void CreateEnemy()
    {
        Enemy instance = enemyFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = spawnZone.SpawnPoint;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        instance.SetColor(Random.ColorHSV(
            hueMin: 0f, hueMax: 1f,
            saturationMin: 0.5f, saturationMax: 1f,
            valueMin: 0.25f, valueMax: 1f,
            alphaMin: 1f, alphaMax: 1f
        ));
        enemies.Add(instance);        
    }

    public void DestroyEnemy(Enemy enemy)
    {
        if (enemies.Count > 0)
        {
            int index = enemies.IndexOf(enemy);
            enemyFactory.Reclaim(enemy);
            int lastIndex = enemies.Count - 1;
            enemies[index] = enemies[lastIndex];
            enemies.RemoveAt(lastIndex);
        }
    }
}
