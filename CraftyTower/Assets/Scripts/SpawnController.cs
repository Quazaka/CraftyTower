using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour {

    private GameObject Tower;
    private GameObject[] spawns;    

    private Vector3 enemyHeight;
    private bool gameOver;

    public GameObject enemyPrefab;

    //enemy hp placeholder //TODO Retrive hp from calcEnemyHP()
    private float hp = 10;

    //Unit spawn rate
    public float spawnRate = 1.0f; // wait one second before spawning next unit
    public float spawnRateModifier = 1.1f; // make units spawn 10% faster each wave

    // Wave timers
    private float waveTime = 10; // 10 second wave
    private float waveWait = 5; // wait 5 seconds before starting new wave    

	// Use this for initialization
	void Start () {
        Tower = GameObject.FindGameObjectWithTag("Tower");

        // Fill spawn array and get enemy height.
        spawns = GameObject.FindGameObjectsWithTag("Spawn");
        enemyHeight = new Vector3(0, enemyPrefab.transform.localScale.y, 0);
        
        StartCoroutine(StartNextWave());
	}

    void Update()
    {
        // When tower is dead destroy all creeps - bool to prevent looping
        if (Tower == null && !gameOver)
        {
            KillAllCreeps();
        }
    }

    IEnumerator StartNextWave()
    {        
        while (true)
        {   
            // Resetting time passed when starting new wave         
            float timePassed = 0;
            float waveStart = Time.time;

            Debug.Log("Starting new wave");

            while (timePassed < waveTime && !gameOver)
            {
                SpawnNext();

                // calculate time passed from start of wave;
                timePassed = Time.time - waveStart;

                yield return new WaitForSeconds(spawnRate);
            }

            // modifying spawnrate after each wave
            // TODO make a function that modifies health, movespeed, damage etc using the interface.
            spawnRate /= spawnRateModifier;

            Debug.Log("Wave over");
            yield return new WaitForSeconds(waveWait);           
        }        
    }

    // Spawn next unit in a random spawn
    void SpawnNext()
    {        
        // Choose a random spawn for next unit
        GameObject spawn = spawns[Random.Range(0, spawns.Length)];
        Vector3 spawnPos = spawn.transform.position;

        /* If the spawn is placed along the x-axis
            place the unit randomly along the z-axis */

        //TODO fix this, kinda hacky and only works if spawns are moved along either the x or z-axis, not both.
        if (spawn.transform.position.x != 0)
        {
            float x = spawn.transform.position.x;
            spawnPos.z = Random.Range(-x, x);
        }
        else // spawn placed along z-axis, spawn along x-axis.
        {
            float z = spawn.transform.position.z;
            spawnPos.x = Random.Range(-z, z);
        }

        // Spawn creeps in random spawn and make them children of that spawn
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos + (enemyHeight / 2), Quaternion.identity) as GameObject;
        spawnedEnemy.transform.parent = spawn.transform;

        //Set hp using IHealth
        IHealth enemyHealth = spawnedEnemy.GetComponent<Enemy>();
        enemyHealth.health = calcEnemyHP();
        enemyHealth.futureHealth = calcEnemyHP();
    }


    //Calculate enemy base hp based on wave
    float calcEnemyHP()
    {
        return hp;
        //TODO Implement claculations to predict enemy hp based on the wave number

    }
    void KillAllCreeps()
    {
        gameOver = true;

        // For each spawn, destroy all it's children
        foreach (GameObject spawn in spawns)
        {
            Transform child = spawn.GetComponentInChildren<Transform>();
            Destroy(child.gameObject);
        }        
    }
}
