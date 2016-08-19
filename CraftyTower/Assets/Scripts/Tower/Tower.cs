using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamage, IExperience {

    public delegate void GameOver();
    public static event GameOver onGameOver;

    public delegate void BaseEnemy();
    public static event BaseEnemy OnEnemyKill;

    // TODO: Make the healthbar use its own script

    #region Variables

    [SerializeField] // Have this field as private but force unity to show it in the editor
    // TODO: We should practice this and use more propteries to ensure encapsulation 
    private float startingHealth = 100;
    private float currentHealth;

    [SerializeField]
    private Image healthContent;
    [SerializeField]
    private Text healthText;

    private IWave Wave;

    private int gold = 0; // The amount of gold to spend
    private int currentExp = 0; // The current amount of experience earned
    private int expToLevel = 20; // The amount of experience needed to level up
    private int towerLevel = 1; // The level of the tower
    private bool levelUp = false; // Did we just level up the tower?

    #endregion

    int IExperience.experience
    {
        set
        {
            CalculateExperience(value);
        }
    }


    // Use this for initialization
    void Start ()
    {
        Wave = GameObject.FindGameObjectWithTag("SpawnControl").GetComponent<Spawner>();

        currentHealth = startingHealth;
        setHealthText();
    }

    // Update is called once per frame
    void Update ()
    {
        if (OnEnemyKill != null)
        {
            OnEnemyKill();
            if (levelUp)
            {
                towerLevel++;
                levelUp = false;
                currentExp -= expToLevel; // if you have more exp than required is it carried over
                expToLevel *= Wave.level; // might result in some levels requiring the same exp, depending on how much exp each enemy gives
                // fine for now, but leave this debug just in case (lvl 1 and 2 both requires 20 exp, because level up is gotten at last creep in wave - before next wave starts)        
                Debug.Log("Current level is:  " + towerLevel + " EXP TO NEXT LVL: " + expToLevel);
            }
        }
    }

    // From Damage interface
    public float damage
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

        //Deactiavte towerobject if health is below zero
        if (currentHealth <= 0)
        {
            if (onGameOver != null)
            {
                Debug.Log("Dead");
                onGameOver();
            }
        }

        // map the health to the sprite and set the health text
        healthContent.fillAmount = mapHealth();
        setHealthText();
    }

    // float value used to map the health to the fillAmount of the sprite(goes from 0 - 1)
    private float mapHealth()
    {
        return currentHealth / startingHealth;
    }

    // Health is shown as text on the healthbar
    private void setHealthText()
    {        
        // Use round because don't want any floating points
        healthText.text = Mathf.Round(currentHealth) + "/" + startingHealth;
    }


    private void CalculateExperience(float expGain)
    {
        currentExp += (int)(expGain * Wave.level);
        if (currentExp >= expToLevel)
        {
            levelUp = true;
        }
    }
}
