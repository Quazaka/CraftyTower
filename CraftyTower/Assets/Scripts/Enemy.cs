using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamage, IHealth {

    private GameObject Tower;
    private Vector3 towerPos;
    private IDamage target;
    private bool stop = false;


    public float hp;
    public float futureHp;
    public int attackDmg = 1;
    public float attackRate = 1f;
    private float nextAttack = 0; 
    public float moveSpeed = 5f;

    //IDamage
    public float damage
    {
        set { TakeDamage(value); }
    }

    //IHealth - real health
    public float health
    {
        get { return hp; } //Read enemy hp
        set { hp = value; } //Set enenmy base spawming hp. //TODO{Overwride in subclasses}
    }

    //IHealth - future health, used to prevent overkill
    public float futureHealth
    {
        get { return futureHp; }
        set { futureHp = value; }
    }


    // Use this for initialization
    void Start ()
    {
        // Find tower object
        Tower = GameObject.FindGameObjectWithTag("Tower");
        // If it exist make sure the creeps move along the ground.
        if (Tower != null)
        {
            towerPos = Tower.transform.position;
            towerPos.y -= (Tower.transform.localScale.y / 2);
            towerPos.y += (this.transform.localScale.y / 2);
        }
        // if not destroy the creep
        else
        {
            Destroy(gameObject);
        }
        
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        // if we haven't reached tower - keep moving
        if (!stop)
        {
            Move();
        }
	}


    //Take damage from bullet
    void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Move the creep towards the tower in real time
    void Move()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, towerPos, moveSpeed * Time.deltaTime);
    }

    // Stop creep when hitting tower
    void OnTriggerEnter(Collider co)
    {
        if(co.name == "Tower")
        {
            stop = true;            
        }
    }

    // Attack tower while creep is still in trigger
    void OnTriggerStay(Collider co)
    {
        // Attack tower
        if (co.name == "Tower")
        {          
            target = co.GetComponent<Tower>();
            if (Time.time > nextAttack)
            {
                target.damage = attackDmg;
                nextAttack = Time.time + attackRate;
            }         
        }
    }
}
