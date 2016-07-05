using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour, IWaveLevel, IWaveEnemyCountLeft, IWaveEnemyType, IGameOver {

    public delegate void WaveToggle();
    public static event WaveToggle toggleWave;

    [SerializeField]
    public GameObject normalEnemyPrefab;
    [SerializeField]
    public GameObject fastEnemyPrefab;
    [SerializeField]
    public GameObject bossEnemyPrefab;

    private GameObject[] spawns;
    private Vector3 normalEnemyHeight;
    private Vector3 fastEnemyHeight;
    private Vector3 bossEnemyHeight;

    private int waveLevel;

    // GameOver bool
    private bool _isGameOver;
    [SerializeField]
    private float spawnRate; 
    private float waveTime; 
    private float waveWait;
    [SerializeField]
    private int aliveEnemies;
    private string enemyType;
    [SerializeField]
    private int enemiesLeftToSpawn;

    //Interface implenementations
    int IWaveLevel.waveLevel{ get{ return waveLevel; } }
    int IWaveEnemyCountLeft.WaveEnemyCountLeft { get { return aliveEnemies; } }
    string IWaveEnemyType.WaveEnemyType { get { return enemyType; } }  

    public bool isGameOver
    {
        get { return _isGameOver; }
        set { _isGameOver = value; }
    }


    // Use this for initialization
    void Start ()
    {
        Initialize();
        FillSpawnArray();
        StartCoroutine(CheckEnemiesLeft());
    }


    //Initialize constants 
    private void Initialize()
    {
        waveTime = 20; // waveTime second wave
        waveWait = 5; // wait waveWait seconds before starting new wave  
        spawnRate = 0.5f; // wait spawnRate second before spawning next unit
        waveLevel = 1; // current wave level
        aliveEnemies = 0; // current number of living enemies
        enemyType = "Normal"; // current enemy type
        enemiesLeftToSpawn = 0;
    }

    // Fill spawn array and get enemy height.
    private void FillSpawnArray()
    {
        spawns = GameObject.FindGameObjectsWithTag("Spawn");
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        while (true)
        {
            while (!_isGameOver && aliveEnemies == 0)
            {
                // Resetting time passed when starting new wave         
                float timePassed = 0;
                float waveStart = Time.time;
                GameObject enemyPrefab = DecideEnemyType();

                Debug.Log("Starting new wave: " + waveLevel);
                while (timePassed < waveTime && !_isGameOver && enemiesLeftToSpawn != 0)
                {
                    enemiesLeftToSpawn--;

                    //Find place to spawn unit and spawn it there
                    FindPositionAndSpawn(enemyPrefab);

                    // calculate time passed from start of wave;
                    timePassed = Time.time - waveStart;

                    yield return new WaitForSeconds(spawnRate);
                }
                Debug.Log("Wave over");
                waveLevel++;
                yield return new WaitForSeconds(waveWait);
            }
            yield return new WaitForSeconds(1);
        }
    }

    // Spawn next unit in a random spawn
    void FindPositionAndSpawn(GameObject enemyPrefab)
    {
         // Choose a random spawn for next unit
        GameObject spawn = spawns[UnityEngine.Random.Range(0, spawns.Length)];
        Vector3 spawnPos = spawn.transform.position;

        spawnPos = GetRandomPosition(spawn, spawnPos);
        // Spawn creeps in random spawn and make them children of that spawn
        SpawnEnemy(spawn, spawnPos, enemyPrefab);
    }

    //Deside what type of enemy, how many and how fast to spawn them based on wave level
    private GameObject DecideEnemyType()
    {
        GameObject enemyPrefab;

        if ((float)(waveLevel % 10) == 0)
        {
            enemyPrefab = bossEnemyPrefab;
            enemyType = "Boss";
            enemiesLeftToSpawn = 1;

            spawnRate = (waveTime / enemiesLeftToSpawn); // Ensure every monster will spawn whitin waveTime secounds.
        }
        else if (waveLevel % 3 == 0)
        {
            enemyPrefab = fastEnemyPrefab;
            enemyType = "Fast";
            enemiesLeftToSpawn = 40;

            spawnRate = (waveTime / enemiesLeftToSpawn);
        }
        else
        {
            enemyPrefab = normalEnemyPrefab;
            enemyType = "Normal";
            enemiesLeftToSpawn = 20;

            spawnRate = (waveTime / enemiesLeftToSpawn);
        }

        return enemyPrefab;
    }

    //Spawn the actual enemy
    private void SpawnEnemy(GameObject spawn, Vector3 spawnPos, GameObject enemyPrefab)
    {
        Vector3 enemyHeight = new Vector3(0, enemyPrefab.transform.localScale.y, 0);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos + (enemyHeight / 2), Quaternion.identity) as GameObject;
        spawnedEnemy.transform.parent = spawn.transform;
    }

    //Get random position for enemy to spawn
    private Vector3 GetRandomPosition(GameObject spawn, Vector3 spawnPos)
    {
        /* If the spawn is placed along the x-axis
        place the unit randomly along the z-axis */

        //TODO fix this, kinda hacky and only works if spawns are moved along either the x or z-axis, not both.
        if (spawn.transform.position.x != 0)
        {
            float x = spawn.transform.position.x;
            spawnPos.z = UnityEngine.Random.Range(-x, x);
        }
        else // spawn placed along z-axis, spawn along x-axis.
        {
            float z = spawn.transform.position.z;
            spawnPos.x = UnityEngine.Random.Range(-z, z);
        }
        return spawnPos;
    }

    //Check how many enemies there are left with event
    IEnumerator CheckEnemiesLeft()
    {
        while (true)
        {
            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            aliveEnemies = Enemies.Length;
            yield return new WaitForSeconds(0.5f);
        }

    }
}
