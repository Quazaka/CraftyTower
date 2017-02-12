using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamage, IExperience {

    public delegate void GameOver();
    public static event GameOver onGameOver;

    public delegate void BaseEnemy();
    public static event BaseEnemy OnEnemyKill;

    #region Variables

    [SerializeField] // Have this field as private but force unity to show it in the editor
    // TODO: We should practice this and use more propteries to ensure encapsulation 
    private float startingHealth = 100;
    private float currentHealth;

    // TODO: Make the healthbar use its own script
    [SerializeField]
    private Image healthContent;
    [SerializeField]
    private Image healthIcon; //The Image to hold the current icon
    private Sprite[] sprites; //All icons for health
    [SerializeField]
    private Text healthText;
    private string texturePath = "Sprites/HealthBar/Icons"; //Path in resources for health icons

    private bool shake = false, left = true; //toggle shake on/off
    private float shaketime = 0;

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

        //Set the health and get all the sprites for the healthIcon
        currentHealth = startingHealth;
        sprites = Resources.LoadAll<Sprite>(texturePath);
        healthIcon.sprite = sprites[0];

        UpdateHealthUI();
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
                currentExp -= expToLevel; // if you have more exp than required it is carried over
                expToLevel *= Wave.level; // might result in some levels requiring the same exp, depending on how much exp each enemy gives
                // fine for now, but leave this debug just in case (lvl 1 and 2 both requires 20 exp, because level up is gotten at last creep in wave - before next wave starts)        
                Debug.Log("Current level is:  " + towerLevel + " EXP TO NEXT LVL: " + expToLevel);
            }
        }

        if (shake)
        {
            StartCoroutine(ShakeHeart());
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
                
        UpdateHealthUI();
    }

    // Health is shown as text on the healthbar
    private void UpdateHealthUI()
    {
        // map the health to the sprite and set the health text - starts at 1 goes to 0
        healthContent.fillAmount = currentHealth / startingHealth;       
        // Use round because don't want any floating points
        healthText.text = Mathf.Round(currentHealth) + "/" + startingHealth;

        //Lerp the healthcolor from max to half health, between healthContent.color and yellow
        if (healthContent.fillAmount > 0.5)
        {
            healthContent.color = Color.Lerp(healthContent.color, Color.yellow, Time.deltaTime);
        }
        //Lerp the healthcolor from half health to death, between healthContent.color and red
        else if (healthContent.fillAmount < 0.5)
        {
            healthContent.color = Color.Lerp(healthContent.color, Color.red, Time.deltaTime);
        }

        // Change the healthIcon based on the amount of health
        if (healthContent.fillAmount == 0.75)
        {
            healthIcon.sprite = sprites[1];
            shake = true;
        }
        else if (healthContent.fillAmount == 0.5)
        {
            healthIcon.sprite = sprites[2];
            shake = true;
        }
        else if (healthContent.fillAmount == 0.25)
        {
            healthIcon.sprite = sprites[3];
            shake = true;
        }
        else if(healthContent.fillAmount <= 0)
        {
            healthIcon.sprite = sprites[4];
        }
    }

    //Shakes the heart icon from side to side
    private IEnumerator ShakeHeart()
    {
        if (shaketime < 0.5f)
        {
            if (left) //shake left
            {
                healthIcon.transform.Rotate(Vector3.forward, 1);
                yield return new WaitForSeconds(0.05f); //Small wait so we can actually see the shaking
                left = false;                
            }
            else //shake right
            {
                healthIcon.transform.Rotate(Vector3.back, 1);
                yield return new WaitForSeconds(0.05f);
                left = true;                
            }
            
            shaketime += Time.deltaTime;
        }
        else
        {
            healthIcon.transform.rotation = Quaternion.identity; //Reset heart position
            shake = false;
            shaketime = 0;
        }
        yield return null;
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
