using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

    private GameObject Tower;
    private GameObject[] spawns;
    private GameObject curSpawn;
    private Vector3 spawnPos;

    private Vector3 enemyHeight;
    private bool gameOver;

    public GameObject enemyPrefab;

    public float spawnRate = 1.0f;

	// Use this for initialization
	void Start () {
        Tower = GameObject.FindGameObjectWithTag("Tower");

        // Fill spawn array and get enemy height.
        spawns = GameObject.FindGameObjectsWithTag("Spawn");
        enemyHeight = new Vector3(0, enemyPrefab.transform.localScale.y, 0);

        InvokeRepeating("SpawnNext", spawnRate, spawnRate);
	}

    void Update()
    {
        // When tower is dead destroy all creeps - bool to prevent looping
        if (Tower == null && !gameOver)
        {
            KillAllCreeps();
        }
    }

    // Spawn creeps in random spawn and make them children of that spawn
    void SpawnNext()
    {
        ChooseSpawn();
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos + (enemyHeight / 2), Quaternion.identity) as GameObject;
        spawnedEnemy.transform.parent = curSpawn.transform;
    }

    // Choose a random spawn 
    void ChooseSpawn()
    {
        curSpawn = spawns[Random.Range(0, spawns.Length)];
        spawnPos = curSpawn.transform.position;

        // Check on which axis the creep is spawned along
        // Making sure the creeps is spawned randomly inside spawn
        if (curSpawn.transform.position.x != 0)
        {
            float x = curSpawn.transform.position.x;
            spawnPos.z = Random.Range(-x, x);
        }
        else
        {
            float z = curSpawn.transform.position.z;
            spawnPos.x = Random.Range(-z, z);
        }        
    }

    void KillAllCreeps()
    {
        // Cancel all invoked calls - stop spawning enemies
        CancelInvoke();
        gameOver = true;

        // For each spawn, destroy all it's children
        foreach (GameObject spawn in spawns)
        {
            Transform child = spawn.GetComponentInChildren<Transform>();
            Destroy(child.gameObject);
        }
    }
}
