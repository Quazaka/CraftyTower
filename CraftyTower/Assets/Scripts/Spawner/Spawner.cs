using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CraftyTower.Spawner;

public class Spawner : MonoBehaviour, IWave, IGameOver
{
    #region Variables

    private GameObject[] spawns; // An array of possible spawnpoints (Child objects of the Spawner)
    [SerializeField]
    private GameObject[] enemyPrefabs;// An array of the enemy prefabs

    [SerializeField]
    private SpawnMode spawnMode = SpawnMode.Wave; // The type of spawning
    private EnemyTypes enemyType = EnemyTypes.Normal; // The type of enemy to spawn;

    public bool spawn = true; // Decide whether the spawner should spawn or not
    private bool spawnWave; // Used to determine whether a new wave should be spawned
    private bool checkEnemiesAlive; // Used to determine if we should check if there are still enemies alive
    
    private int enemiesToSpawn; // Total number of enemies to spawn
    private int enemiesSpawned; // Current number of enemies spawned - also counting dead enemies
    
    private float spawnRate = 0.5f; // wait spawnRate seconds before spawning next enemy   
    private float waveWait = 5f; // wait waveWait seconds before starting new wave - when spawntype is set to Wave
    private float waveTime = 20f; // A wave lasts waveTime second when spawntype is set to TimedWave

    [SerializeField]
    private int _level = 0; // The current level (corresponds to number of waves spawned)
    private bool _isGameOver; // used to check whether the game is over or not
    [SerializeField]
    private int _enemiesAlive; // Current number of alive enemies

    #region Interface Implementation
    int IWave.level { get { return _level; } }
    int IWave.enemyCountLeft { get { return _enemiesAlive; } set { _enemiesAlive = value; } }
    string IWave.enemyType { get { return enemyType.ToString(); } }

    bool IGameOver.isGameOver
    {
        get { return _isGameOver; }
        set { _isGameOver = value; }
    }
    #endregion

    #endregion

    // Use this for initialization
    void Start ()
    {
	    if (spawns == null || enemyPrefabs == null)
        {
            spawns = GameObject.FindGameObjectsWithTag("Spawn");
            enemyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemy");
        }
        // TODO: Do some Object Pooling tricks here before starting the spawning
        // https://blogs.msdn.microsoft.com/dave_crooks_dev_blog/2014/07/21/object-pooling-for-unity3d/
        StartCoroutine("DoSpawn");
    }

    /// <summary>
    /// Coroutine that uses a switch-case to determine what logic to use for wave handling
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoSpawn()
    {        
        UpdateWaveStats();        
        while (spawn && !_isGameOver)
        {            
            switch (spawnMode)
            {
                case SpawnMode.Single:
                    StartCoroutine(SpawnSingle());
                    yield return new WaitForSeconds(spawnRate);
                    break;

                case SpawnMode.Wave:                  
                    StartCoroutine(SpawnWave());
                    yield return new WaitForSeconds(spawnRate);
                    break;

                case SpawnMode.TimedWave:
                    StartCoroutine(SpawnTimedWaved());
                    yield return new WaitForSeconds(spawnRate);
                    break;

                default:
                    spawn = false;
                    break;
            }
            yield return new WaitForEndOfFrame();
        }        
    }

    #region SpawnHandling
    /// <summary>
    /// Spawns an enemy in a random position in a random spawn
    /// </summary>
    private void SpawnEnemy()
    {
        GameObject curSpawn = GetRandomSpawn();
        Vector3 spawnPos = GetRandomSpawnPosition(curSpawn);        

        if (enemyPrefabs[(int)enemyType] != null)
        {
            // Instantiate the enemy and make it a child of the spawn in which it was spawned
            Transform spawnedEnemy = Instantiate(enemyPrefabs[(int)enemyType].transform, spawnPos, Quaternion.identity) as Transform;
            spawnedEnemy.parent = curSpawn.transform;
            // Increase the total number of enemies spawned and the number of enemies alive
            enemiesSpawned++;
            _enemiesAlive++;
        }
        else
        {
            Debug.LogError("Error trying to spawn enemy of type " + enemyType.ToString() + " on spawner " + curSpawn.name + " - No unit set");
            spawn = false;
        }        
    }

    /// <summary>
    /// For all the existing spawns, pick and return a random one
    /// </summary>
    /// <returns>A random GameObject spawn from an array of spawns</returns>
    private GameObject GetRandomSpawn()
    {
        return spawns[Random.Range(0, spawns.Length)];
    }

    /// <summary>
    /// Get a random position within a specified spawn
    /// </summary>
    /// <param name="spawn">The spawn to find a random position in</param>
    /// <returns>A random position within the specified spawn</returns>
    private Vector3 GetRandomSpawnPosition(GameObject spawn)
    {
        Vector3 spawnPos = spawn.transform.position;

        // TODO fix this, kinda hacky and only works if spawns are moved along either the x or z-axis, not both.
        if (spawnPos.x != 0)
        {
            float x = spawnPos.x;
            spawnPos.z = Random.Range(-x, x);
        }
        else // spawn placed along z-axis, spawn along x-axis.
        {
            float z = spawnPos.z;
            spawnPos.x = Random.Range(-z, z);
        }
        return spawnPos;

    }

    /// <summary>
    /// Decides which type of enemy to spawn based on the current wave level
    /// </summary>
    private void DecideEnemyType()
    {
        if ((_level % 10) == 0) // spawn a boss every 10th level
        {
            enemyType = EnemyTypes.Boss;
            enemiesToSpawn = 1;
        }
        else if ((_level % 3) == 0) // spawn fast enemies every 3rd level
        {
            enemyType = EnemyTypes.Fast;
            enemiesToSpawn = 40;
        }
        else // spawn normal enemies when no other condition is met
        {
            enemyType = EnemyTypes.Normal;
            enemiesToSpawn = 20;
        }

        // Ensure every enemy will spawn within waveTime seconds.
        spawnRate = (waveTime / enemiesToSpawn);
    }
    #endregion
    
    #region Wave Coroutines
    //--- SPAWN SINGLE
    private IEnumerator SpawnSingle()
    {
        if (enemiesSpawned >= enemiesToSpawn)
        {
            // if we have spawned all enemies in this mode, disable the spawn
            // TODO: Make a button or something that activates the spawn again (Spawn next wave thing)
            // TODO: Have the button handle the two lines of code below
            spawn = false;
            UpdateWaveStats();
        }
        else
        {
            SpawnEnemy();
            yield return new WaitForEndOfFrame();
        }
    }

    //--- SPAWN WAVE
    private IEnumerator SpawnWave()
    {
        if (spawnWave)
        {
            SpawnEnemy();

            if (enemiesSpawned == enemiesToSpawn)
            {
                // if the we have spawned all enemies for the wave, but not killed them all, do not spawn another wave.      
                spawnWave = false;
                checkEnemiesAlive = true;
                Debug.Log("Spawned all enemies");
            }            
        }
        if (checkEnemiesAlive)
        {
            // if all spawned enemies are dead, get ready to spawn a new wave
            if (_enemiesAlive <= 0)
            {               
                checkEnemiesAlive = false;
                Debug.Log("All enemies are dead -  Wave " + _level + " over");
                yield return new WaitForSeconds(waveWait);
                // We are not ready to spawn a new wave unless we've waited for it                
                UpdateWaveStats();                
            }            
        }               
    }

    //--- SPAWN TIMED WAVED
    private IEnumerator SpawnTimedWaved()
    {
        if (spawnWave)
        {
            SpawnEnemy();

            if (enemiesSpawned == enemiesToSpawn)
            {
                // if we have spawned all the enemies, wait for waveWait seconds
                spawnWave = false;
                yield return new WaitForSeconds(waveWait);                
                // When the wait is over, update the stats and get ready to spawn a new wave
                UpdateWaveStats();
            }            
        }
    }
    #endregion

    /// <summary>
    /// Updates the current wave level, resets the number of enemies spawned
    /// and decides whether a new wave should be spawned
    /// </summary>
    public void UpdateWaveStats()
    {
        _level++;
        enemiesSpawned = 0;
        if (spawnMode != SpawnMode.Single)
        {
            spawnWave = true;
        }
        // TODO: Something when spawnMode is single (UI stuff)

        // Decide the enemyType and use the int value from the enemyType enum to determine what prefab to use             
        DecideEnemyType();
        Debug.Log("Started wave: " + _level + ". " + enemyType.ToString() + " enemies incoming");
    }
}
