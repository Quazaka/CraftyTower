using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

    private GameObject[] spawns;
    private GameObject curSpawn;
    private Vector3 spawnPos;

    private Vector3 enemyHeight;
    public GameObject enemyPrefab;

    public float spawnRate = 1.0f;

	// Use this for initialization
	void Start () {
        spawns = GameObject.FindGameObjectsWithTag("Spawn");
        enemyHeight = new Vector3(0, enemyPrefab.transform.localScale.y, 0);

        InvokeRepeating("SpawnNext", spawnRate, spawnRate);
	}

    void SpawnNext()
    {
        ChooseSpawn();
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos + (enemyHeight / 2), Quaternion.identity) as GameObject;
        spawnedEnemy.transform.parent = curSpawn.transform;
    }

    void ChooseSpawn()
    {
        curSpawn = spawns[Random.Range(0, spawns.Length)];
        spawnPos = curSpawn.transform.position;

        if (curSpawn.transform.position.x != 0)
        {
            spawnPos.z = Random.Range(-19.5f, 19.5f);
        }
        else
        {
            spawnPos.x = Random.Range(-19.5f, 19.5f);
        }        
    }
}
