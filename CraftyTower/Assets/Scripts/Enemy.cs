using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, Damage {

    private GameObject Tower;
    private Vector3 towerPos;

    public int hp = 10;
    public float moveSpeed = 5f;

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

    public int damage
    {
        set {TakeDamage(value);}
    }

    //Take damage from bullet
    void TakeDamage(int damage)
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
