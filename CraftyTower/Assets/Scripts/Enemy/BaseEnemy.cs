using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour, IDamage, IHealth {

    public delegate void WaveUI();
    public static event WaveUI OnEnemySpawned;
    public static event WaveUI OnEnemyHit;

    #region Base Enemy Variables
    private IDamage targetIDamage;
    protected IWave Wave;

    private Transform tower;
    private Vector3 towerPos;

    //[SerializeField]
    //private float attackDmg;
    //[SerializeField]
    //private float attackRate;
    //[SerializeField]
    //private float damageReduction;
    //[SerializeField]
    //private float futureHealthPoints;
    //[SerializeField]
    //private float healthPoints;
    //[SerializeField]
    //private float moveSpeed;
    //[SerializeField]
    //private bool isHitByLightning;

    private float nextAttack = 0;
    private bool stop = false;

    private Color normalColor;
    private Color hitColor = Color.red;
    private Material enemyMaterial;
    #endregion

    #region Get/Set
    protected float AttackDamage { get; set; }
    protected virtual float AttackRate { get; set; }
    protected virtual float MoveSpeed { get; set; }
    // TODO: Implement damage reduction as armor
    protected virtual float DamageReduction { get; set; }
    #endregion

    #region Interface implementation
    //IDamage - Take damage from bullet
    public void TakeDamage(float damage)
    {
        StartCoroutine(ChangeEnemyColorOnHit());
        health -= damage;
    }

    //IHealth - Actual health
    public float health { get; set; }

    //IHealth - future health, used to prevent overkill
    public float futureHealth { get; set; }
    #endregion

    #region Virtual Methods
    protected virtual int CalculateHealth(float health, int wave)
    {
        return (int)Mathf.Pow(wave, health);
    }

    protected virtual float CalculateDamage(float damage, int wave)
    {
        return damage * wave;
    }
    #endregion

    // Use this for initialization
    protected virtual void Start ()
    {
        //Store current wavelevel
        Wave = GameObject.FindGameObjectWithTag("SpawnControl").GetComponent<Spawner>();

        tower = GameObject.FindGameObjectWithTag("Tower").transform.root; // Using root to ensure highest element in the hierachy

        // If it exist make sure the creeps move along the ground.
        if (tower != null)
        {
            towerPos = tower.transform.position;
            towerPos.y = (this.transform.localScale.y / 2); // Set the y-coordinate of the tower to be equal to half the height of the spawned enemy

            // Set tower as target for iDamage to use
            targetIDamage = tower.GetComponent<Tower>();

            // Get the material and set a reference to the enemy's normal color so we can change color on hit
            enemyMaterial = gameObject.GetComponent<Renderer>().material;
            normalColor = enemyMaterial.color;
        }
        // if not destroy the enemy
        else
        {
            Wave.enemiesAlive--;
            Destroy(gameObject);
        }
    }
     
    protected void OnSpawn()
    {
        if (OnEnemySpawned != null)
        {
            OnEnemySpawned();
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

    // Move the creep towards the tower in real time
    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, towerPos, MoveSpeed * Time.deltaTime);
    }

    // Stop creep when hitting tower
    protected void OnTriggerEnter(Collider co)
    {
        if(co.name == "TowerBase")
        {
            stop = true;            
        }
    }

    // Attack tower while creep is still in trigger
    private void OnTriggerStay(Collider co)
    {
        // Attack tower
        if (co.name == "TowerBase")
        {
            if (Time.time > nextAttack)
            {
                targetIDamage.TakeDamage(AttackDamage);
                nextAttack = Time.time + AttackRate;
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
        if (health <= 0)
        {
            Wave.enemiesAlive--;
            if (OnEnemyHit != null)
            {
                OnEnemyHit();
            }
            Destroy(gameObject);
        }
    }
    #endregion

    //Shortly change color on enemy when hit.
    IEnumerator ChangeEnemyColorOnHit()
    {
        // Change color when hit - then wait
        enemyMaterial.color = hitColor;
        yield return new WaitForSeconds(0.10f);
        
        // Change color back to normal
        enemyMaterial.color = normalColor;
    }
}
