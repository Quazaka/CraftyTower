using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour {

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

    void OnEnable()
    {
        Tower.OnHealthLost += UpdateHealthUI;
    }

    void OnDisable()
    {
        Tower.OnHealthLost -= UpdateHealthUI;
    }

    void Start()
    {
        // Load all sprites and set the first sprite
        sprites = Resources.LoadAll<Sprite>(texturePath);
        healthIcon.sprite = sprites[0];
    }

    void Update()
    {
        if (shake)
        {
            StartCoroutine(ShakeHeart());
        }
    }

    // Health is shown as text on the healthbar
    private void UpdateHealthUI(float currentHealth, float startingHealth)
    {
        // map the health to the sprite and set the health text - starts at 1 goes to 0
        healthContent.fillAmount = currentHealth / startingHealth;
        // Use round because don't want any floating points - and clamp because we don't want to go below 0 or above startingHealth
        Mathf.Clamp(currentHealth, 0, startingHealth);
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

        switch ((int)currentHealth)
        {
            case 75:
                healthIcon.sprite = sprites[1];
                shake = true;
                break;
            case 50:
                healthIcon.sprite = sprites[2];
                shake = true;
                break;
            case 25:
                healthIcon.sprite = sprites[3];
                shake = true;
                break;
            case 0:
                healthIcon.sprite = sprites[4];
                break;
            default:
                break;
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
}
