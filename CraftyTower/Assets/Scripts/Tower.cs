using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour, Damage {

    public GameObject weaponPrefab;

    public int health = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("space"))
        {
            GameObject wep = Instantiate(weaponPrefab, transform.position, Quaternion.identity) as GameObject;
            Debug.Log("Space pressed");
        }
	}

    // From Damage interface
    public int damage
    {
        set { TakeDamage(value); }
    }

    // Take damage from enemies
    private void TakeDamage(int damage)
    {
        health -= damage;

        //Deactiavte towerobject if health is below zero
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
