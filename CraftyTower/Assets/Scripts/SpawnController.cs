using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnController : MonoBehaviour, IWaveLevel {
   
    private GameObject[] spawns;
    private Vector3 normalEnemyHeight;
    private Vector3 fastEnemyHeight;

    public GameObject normalEnemyPrefab;
    public GameObject fastEnemyPrefab;

    private int waveLevel = 1;

    //Unit spawn rate
    public float spawnRate = 0.5f; // wait spawnRate second before spawning next unit

    // Wave timers
    private float waveTime = 20; // waveTime second wave
    private float waveWait = 5; // wait waveWait seconds before starting new wave    

    int IWaveLevel.waveLevel{ get{ return waveLevel; } }

    // Use this for initialization
    void Start ()
    {        
        // Fill spawn array and get enemy height.
        spawns = GameObject.FindGameObjectsWithTag("Spawn");
        normalEnemyHeight = new Vector3(0, normalEnemyPrefab.transform.localScale.y, 0);
        fastEnemyHeight = new Vector3(0, fastEnemyPrefab.transform.localScale.y, 0);


        StartCoroutine(StartNextWave());
	}

    IEnumerator StartNextWave()
    {        
        while (true)
        {
            // Resetting time passed when starting new wave         
            float timePassed = 0;
            float waveStart = Time.time;

            Debug.Log("Starting new wave: " + waveLevel);

            while (timePassed < waveTime)
            {
                SpawnNext(RetrunRandomPrefab(normalEnemyPrefab, fastEnemyPrefab));

                // calculate time passed from start of wave;
                timePassed = Time.time - waveStart;

                yield return new WaitForSeconds(spawnRate);
            }
            Debug.Log("Wave over");
            waveLevel++;
            yield return new WaitForSeconds(waveWait);           
        }        
    }

    // Spawn next unit in a random spawn
    void SpawnNext(GameObject enemySpawnPrefab)
    {        
        // Choose a random spawn for next unit
        GameObject spawn = spawns[UnityEngine.Random.Range(0, spawns.Length)];
        Vector3 spawnPos = spawn.transform.position;

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

        // Spawn creeps in random spawn and make them children of that spawn
        GameObject spawnedEnemy = Instantiate(enemySpawnPrefab, spawnPos + (normalEnemyHeight / 2), Quaternion.identity) as GameObject;
        spawnedEnemy.transform.parent = spawn.transform;
    }

    private GameObject RetrunRandomPrefab(GameObject p1, GameObject p2)
    {
        int i = Random.Range(1, 3);
        if (i == 1)
        {
            return p1;
        }
        else
        {
            return p2;
        }
        
    }
}
