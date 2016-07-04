using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

    private GameObject Tower;
    private GameObject[] spawns;

    private bool gameOver;

    // Use this for initialization
    void Start ()
    {
        Tower = GameObject.FindGameObjectWithTag("Tower");
    }
	
	// Update is called once per frame
	void Update ()
    {
        // When tower is dead destroy all creeps - bool to prevent looping
        if (Tower == null && !gameOver)
        {
            KillAllCreeps();
        }
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
