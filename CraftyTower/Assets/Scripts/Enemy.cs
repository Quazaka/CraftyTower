using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamage, IHealth {

    private GameObject Tower;
    private Vector3 towerPos;

    public float hp;
    public float moveSpeed = 5f;
     

    //Damage interface
    public float damage
    {
        set { TakeDamage(value); }
    }

    //Health interface
    public float health
    {
        get {return hp; } //Read enemy hp
        set { hp = value;} //Set enenmy base spawming hp. //TODO{Overwride in subclasses}
    }

    // Use this for initialization
    void Start ()
    {
        Tower = GameObject.FindGameObjectWithTag("Tower");
        towerPos = Tower.transform.position;
        towerPos.y -= (Tower.transform.localScale.y / 2);
        towerPos.y += (this.transform.localScale.y / 2);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Move();
	}


    //Take damage from bullet
    void TakeDamage(float damage)
    {
        hp = hp - damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, towerPos, moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider co)
    {
        if( co.name == "Tower" )
        {
            //Tower.TakeDamage();
            Destroy(gameObject);
        }
    }
}
