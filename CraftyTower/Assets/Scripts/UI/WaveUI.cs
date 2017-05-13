using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

    private IWave Wave;
    private IHealth Health;

    [SerializeField]
    private Image healthContent;
    [SerializeField]
    private Text enemyCountText;
    [SerializeField]
    private Text WaveText;

    private float enemyHealth; // Store the enemy health - as we can't use the interface reference when it enemies die
    private int totalEnemies;

    void Start()
    {
        Wave = GameObject.FindGameObjectWithTag("SpawnControl").GetComponent<Spawner>();
    }

    void OnEnable()
    {
        Spawner.OnWaveStart += WaveStarted;
        BaseEnemy.OnEnemySpawned += EnemyChanged;
        BaseEnemy.OnEnemyHit += EnemyChanged;
    }

    void OnDisable()
    {
        Spawner.OnWaveStart -= WaveStarted;
        BaseEnemy.OnEnemySpawned -= EnemyChanged;
        BaseEnemy.OnEnemyHit -= EnemyChanged;
    }

    private void WaveStarted(int enemiesToSpawn)
    {
        WaveText.text = "Wave: " + Wave.level;
        enemyCountText.text = 0 + "/" + enemiesToSpawn;
        totalEnemies = enemiesToSpawn;
    }

    private void EnemyChanged()
    {
        if (Health == null)
        {
            Health = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BaseEnemy>();
            enemyHealth = Health.health;
        } 

        // When enemies die - use the stored reference
        if (Health.health <= 0)
        {
            enemyCountText.text = Wave.enemiesAlive + "/" + totalEnemies;
            healthContent.fillAmount = Wave.enemiesAlive * enemyHealth / Wave.enemiesSpawned * enemyHealth;
        }
        else // When enemies spawn or are hit - use Health.health
        {           
            enemyCountText.text = Wave.enemiesAlive + "/" + totalEnemies;
            healthContent.fillAmount = Wave.enemiesAlive * Health.health / Wave.enemiesSpawned * Health.health;
        }
    }
}
