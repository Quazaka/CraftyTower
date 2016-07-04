using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour, IDamage, IHealth {
    #region initialization
    private IWaveLevel WaveLevel;

    protected GameObject Tower;
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
    protected abstract float CalcuLateDamage(int wave);

    protected virtual void Update()
    {

    }

    // Use this for initialization
    protected virtual void Start ()
    {
        //Store current wavelevel
        WaveLevel = (IWaveLevel)GameObject.FindGameObjectWithTag("SpawnControl").GetComponent<SpawnController>();
        currentWaveLevel = WaveLevel.waveLevel;

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

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
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
