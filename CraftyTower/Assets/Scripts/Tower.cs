using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamage {

    // TODO: Finish healthbar and add a script to show it

    public float health = 100;

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    // From Damage interface
    public float damage
    {
        set { TakeDamage(value); }
    }

    // Take damage from enemies
    private void TakeDamage(float damage)
    {
        health -= damage;

        //Deactiavte towerobject if health is below zero
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
