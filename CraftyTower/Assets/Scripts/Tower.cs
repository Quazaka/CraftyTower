using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamage {

    // TODO: Make the healthbar use its own script

    [SerializeField] // Have this field as private but force unity to show it in the editor
    // TODO: We should practice this and use more propteries to ensure encapsulation 
    private float startingHealth = 100;
    private float currentHealth;

    [SerializeField]
    private Image healthContent;
    [SerializeField]
    private Text healthText;

	// Use this for initialization
	void Start ()
    {        
        currentHealth = startingHealth;
        setHealthText();
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.right, Time.deltaTime + 0.25f);
        transform.Rotate(Vector3.up, Time.deltaTime + 0.25f, Space.World);
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
        
        //Deactiavte towerobject if health is below zero
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
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
        healthText.text = currentHealth + "/" + startingHealth;
    }
}
