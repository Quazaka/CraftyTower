using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

    // Using the interface to the stop spawnController from spawning
    private IGameOver gameOver;

    private GameObject tower;

    public Text restartText;
    public Text gameOverText;

    void Start()
    {
        // Find the spawncontroller script so we can reference the isGameOver through the interface
        gameOver = GameObject.FindGameObjectWithTag("SpawnControl").GetComponent<Spawner>();
        tower = GameObject.FindGameObjectWithTag("Tower");

        restartText.text = "";
        gameOverText.text = "";
    }

    void Update()
    {
        if (gameOver.isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // load first scene in the scenemanager when R is pressed
                SceneManager.LoadScene("Scene_Tower");
                // we could also do this since we do not have any other scenes.
                //SceneManager.LoadScene(0);
            }
        }
    }

    // Have the tower subscribe to the event 
    void OnEnable()
    {
        Tower.onGameOver += EndGame;
    }

    void OnDisable()
    {
        Tower.onGameOver -= EndGame;
    }

    void EndGame()
    {
        // set the interface bool so that the spawnController knows not to spawn more enemies
        gameOver.isGameOver = true;

        // Kill all creeps then destroy the event subscribers
        KillAllCreeps();
        Destroy(tower);
        SetGameOverText();
    }

    void KillAllCreeps()
    {      
        // Find all active enemies
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Destroy all active enemies
        foreach (GameObject enemy in Enemies)
        {
            Destroy(enemy);
        }
    }    
    
    void SetGameOverText()
    {
        gameOverText.enabled = true;
        restartText.enabled = true;

        gameOverText.text = "GAME OVER";
        restartText.text = "Press 'R' to restart";
    }        
}
