using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour, IDamage, IHealth {
    #region initialization
    private IWave Wave;
    private IExperience Experience;

    protected GameObject towerObj;
    private Vector3 towerPos;
    private IDamage target;
    private bool stop = false;

    [SerializeField]
    protected float _attackDmg;
    [SerializeField]
    protected float _attackRate;
    [SerializeField]
    protected float _damageReduction;
    [SerializeField]
    protected float _futureHp;
    [SerializeField]
    protected float _hp;
    [SerializeField]
    protected float _moveSpeed;

    public abstract float hp { get; set; }
    public abstract float futureHp { get; set; }
    public abstract float attackDmg { get; set; }
    public abstract float attackRate { get; set; }
    public abstract float moveSpeed { get; set; }
    public abstract float damageReduction { get; set; }

    protected int currentWaveLevel;
    protected float nextAttack = 0;

    //IDamage
    public float damage
    {
        set { TakeDamage(value); }
    }

    //IHealth - real health
    public float health
    {
        get { return hp; } //Read enemy hp
    }

    //IHealth - future health, used to prevent overkill
    public float futureHealth
    {
        get { return futureHp; }
        set { futureHp = value; }
    }
    #endregion initialization

    //Abstrat methods
    protected abstract int CalculateHp(int wave);
    protected abstract float CalculateDamage(int wave);

    protected virtual void Update()
    {

    }

    // Use this for initialization
    protected virtual void Start ()
    {
        //Store current wavelevel
        Wave = GameObject.FindGameObjectWithTag("SpawnControl").GetComponent<Spawner>();
        currentWaveLevel = Wave.level;

        // Find tower object
        towerObj = GameObject.FindGameObjectWithTag("Tower");
        // If it exist make sure the creeps move along the ground.
        if (towerObj != null)
        {
            // TODO: Should make a empty GameObject at tower-origin and have enemies move towards that
            // instead of a specific part of the tower (tower y-pos is based it's height making enemies float if this isn't done)
            towerPos = towerObj.transform.position;
            towerPos.y -= (towerObj.transform.localScale.y / 2); // Lower the y-coords by the scale of the tower
            towerPos.y += (this.transform.localScale.y / 2); // Increase the y-coords by the scale of the enemy-type
        }
        // if not destroy the creep
        else
        {
            Wave.enemyCountLeft--;
            Destroy(gameObject);
        }

        Experience = GameObject.FindGameObjectWithTag("Tower").GetComponent<Tower>();
    }

    // Update is called once per frame
    protected void FixedUpdate ()
    {
        // if we haven't reached tower - keep moving
        if (!stop)
        {
            Move();
        }
	}

    //Take damage from bullet
    protected void TakeDamage(float damage)
    {
        StartCoroutine(ChangeEnemyColorOnHit());
        hp -= damage;
    }

    // Move the creep towards the tower in real time
    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, towerPos, moveSpeed * Time.deltaTime);
    }

    // Stop creep when hitting tower
    protected void OnTriggerEnter(Collider co)
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

    #region Delegate stuff
    void OnEnable()
    {
        Tower.OnEnemyKill += KillAndUpdate;
    }

    void OnDisable()
    {
        Tower.OnEnemyKill -= KillAndUpdate;
    }

    void KillAndUpdate()
    {        
        if (hp <= 0)
        {
            Wave.enemyCountLeft--;
            Destroy(gameObject);
            // Get experience when enemy is dead
            Experience.experience = 1;
        }
    }
    #endregion


    //Shortly change color on enemy when hit.
    #region OnHit
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
