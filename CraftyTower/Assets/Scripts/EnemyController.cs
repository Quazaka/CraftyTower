using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, Damage {

    public GameObject Tower;
    int hp = 100;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}


    public int Damage
    {
        set {TakeDamage(value);}
    }

    //Take damage from bullet
    void TakeDamage(int damage)
    {

        hp = hp - damage;
    }

    
}
