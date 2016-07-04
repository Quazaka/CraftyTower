using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamage, IHealth {
    #region initialization
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
    #endregion initialization

    // Use this for initialization
    private void Start ()
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
	private void FixedUpdate ()
    {
        // if we haven't reached tower - keep moving
        if (!stop)
        {
            Move();
        }
	}


    //Take damage from bullet
    private void TakeDamage(float damage)
    {
        StartCoroutine(ChangeEnemyColorOnHit());
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Move the creep towards the tower in real time
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, towerPos, moveSpeed * Time.deltaTime);
    }

    // Stop creep when hitting tower
    private void OnTriggerEnter(Collider co)
    {
        if(co.name == "Tower")
        {
            stop = true;            
        }
    }

    // Attack tower while creep is still in trigger
    private void OnTriggerStay(Collider co)
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

    //Shortly change color on enemy when hit.
    #region
    IEnumerator ChangeEnemyColorOnHit()
    {
        Color normalColor = gameObject.GetComponent<Renderer>().material.color;

        SetHitColor();
        yield return new WaitForSeconds(0.10f);
        SetNormalColor(normalColor);
    }

    private void SetHitColor()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        return;
    }

    private void SetNormalColor(Color normalColor)
    {
        gameObject.GetComponent<Renderer>().material.color = normalColor;
        return;
    }
    #endregion  

}
