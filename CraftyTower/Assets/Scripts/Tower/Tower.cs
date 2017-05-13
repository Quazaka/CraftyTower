using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamage {

    public delegate void GameOver();
    public static event GameOver onGameOver;

    public delegate void BaseEnemy();
    public static event BaseEnemy OnEnemyKill;

    public delegate void TowerUI(float currentHealth, float startingHealth);
    public static event TowerUI OnHealthLost;


    #region Variables

    [SerializeField] // Have this field as private but force unity to show it in the editor
    // TODO: We should practice this and use more propteries to ensure encapsulation 
    private float startingHealth;
    private float currentHealth;
    #endregion

    // Use this for initialization
    void Start ()
    {
        //Set the starting health
        startingHealth = 100;
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update ()
    {
        if (OnEnemyKill != null)
        {
            OnEnemyKill();
        }
    }

    // From Damage interface
    public float takeDamage
    {
        set { TakeDamage(value); }
    }

    // Take damage from enemies
    private void TakeDamage(float damage)
    {
        // Lower current health based on damage input
        currentHealth -= damage;
        // Make sure we cant go below 0 health or over startinghealth.
        Mathf.Clamp(currentHealth, 0, startingHealth);

        // Using the delegate to update Tower UI when health is lost
        if (OnHealthLost != null)
        {
            OnHealthLost(currentHealth, startingHealth);
        }

        //Deactiavte towerobject if health is below zero
        if (currentHealth <= 0)
        {
            if (onGameOver != null)
            {
                Debug.Log("Dead");
                onGameOver();
            }
        }
    }
}
