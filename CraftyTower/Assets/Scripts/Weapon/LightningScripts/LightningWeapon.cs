using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class LightningWeapon : BaseWeapon {

    private List<GameObject> IHaveBeenHit = new List<GameObject>(); //List to store enemies already hit by this chain jump
    private List<GameObject> jumpEnemyList = new List<GameObject>(); //List to store enemies that can be jumped to
    private float lifetime = 0.1f; // How long the arch should be displayed
    private float _jumpDamage; // Store damage during jumps
    private float _tempDamage; //Store damage with modifiers

    #region Upgrades
    [Range (0f, 100f)]
    [SerializeField]
    private float _damage = 1; //private constanst as damage
    private float _firerate = 1;
    private float _range = 5;
    private float _JumpChance = 0; // 0 - 100
    private float _damageLossPerJump = 0.5f; // 0.5f = 50% damage loss
    private int _jumpCount = 1; //Number of jumps
    private float _jumpDistance = 0.5f; //Jump Distance
    private bool _haveRodEffect = false;
    private float _rodChance = 0;
    private float _rodDuration = 0;
    private float _rodCooldown = 0;
    private float _rodDamage = 0;

    #region get/set

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public override float Firerate
    {
        get { return _firerate; }
        set { _firerate = value; }
    }

    public override float Range
    {
        get { return _range; }
        set { _range = value; }
    }

    public float JumpDamage
    {
        get { return _jumpDamage; }
        set { _jumpDamage = value; }
    }

    public float JumpChance
    {
        get{ return _JumpChance; }
        set{ _JumpChance = value; }
    }

    public float DamageLossPerJump
    {
        get{ return _damageLossPerJump; }
        set{ _damageLossPerJump = value; }
    }

    public int JumpCount
    {
        get{ return _jumpCount; }
        set{ _jumpCount = value; }
    }

    public float JumpDistance
    {
        get{ return _jumpDistance; }
        set{ _jumpDistance = value; }
    }

    // TODO implement Rod effect in lightning weapon.
    public bool HaveRodEffect
    {
        get{ return _haveRodEffect; }
        set{ _haveRodEffect = value; }
    }

    public float RodChance
    {
        get { return _rodChance; }
        set { _rodChance = value; }
    }

    public float RodDuration
    {
        get{ return _rodDuration; }
        set{ _rodDuration = value; }
    }

    public float RodCooldown
    {
        get{ return _rodCooldown; }
        set{ _rodCooldown = value; }
    }

    public float RodDamage
    {
        get{ return _rodDamage; }
        set{ _rodDamage = value; }
    }
    #endregion

    #region upgrades
    //Calculate damage based on variables
    protected override float CalculateDamageWithVariables()
    {
        return _damage;
    }
    #endregion

    protected override GameObject setTarget(GameObject projectile, GameObject currentTarget)
    {
        Debug.Log("This is an error - Check LightningWeapon.cs setTarget should not be called");
        throw new NotImplementedException(); // Not to be implemented, if called it's a mistake
    }

    //Override shoot
    protected override void Shoot(GameObject currentTarget)
    {
        Debug.Log("Shooting with lightnings");
        //Remove null targets from enemyList
        RemoveNullObjectFromList(enemyList);

        //Instanciate first lightning arc
        LightningAttack(currentTarget, JumpCount);

    }
    void Awake() // This is used instead of Start(), as start runs in the baseclass.
    {
        targetSwitch = 1; //Closest target
    }

    private void LightningAttack(GameObject target, int noJumps)
    {
        List<GameObject> targetList = new List<GameObject>();
        targetList.Add(target);
        IHaveBeenHit.Add(target);

        //If PeformJump Returns true, peform the jump with noJumps, else dont jump and just hit the first enemy.
        int tempNoJump = noJumps;
        if (!PeformJump())
        {
            Debug.Log("Dont jump");
            tempNoJump = 0;
        }
        for (int i = 0; i < tempNoJump; i++)
        {
            Debug.Log("# " + i);
            //Create list with targets to deal damge and draw arch to.
            target = SetNextTarget(target); // Find next target in range of Target
            if(target == null) { break; }
            targetList.Add(target);
            IHaveBeenHit.Add(target);
        }
        CreateLightningArc(targetList); // Create a lightningArc effect from previous target to the next target and apply damage.
        IHaveBeenHit.Clear(); //Clear IHaveBeenHit so the next chain can use it.
        _jumpDamage = _damage; //Set _JumpDamage to euqal _damage so each chain does equal damage.

    }

    private void CreateLightningArc(List<GameObject> targetList)
    {
        DrawArc(targetList); // Draw arch
        int i = 0;

        foreach (GameObject target in targetList)
        {
            if (i == 0) // if first target dont reduce damage.
            {
                DealDamage(target, _jumpDamage); // Deal damage
            } else
            {
                _jumpDamage = _jumpDamage * _damageLossPerJump;
                DealDamage(target, _jumpDamage); // Deal damage with damage reduction
            }
            i++;
        }
    }

    private void DrawArc(List<GameObject> targetList)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        int vertexCount = targetList.Count() + 1;

        lineRenderer.GetComponent<LineRenderer>().enabled = true; //Enable line renderer
        lineRenderer.GetComponent<LineRenderer>().SetWidth(0.05f, 0.05f); // set width of lightning
        lineRenderer.GetComponent<LineRenderer>().SetVertexCount(vertexCount); //How many parts to devide the line into
        lineRenderer.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position); //position of the first vortex

        int i = 1;
        foreach (GameObject target in targetList)
        {
            //var pos = Vector3.Lerp(previousTarget.transform.position, target.transform.position, i / ((float)vertexCount));
            lineRenderer.GetComponent<LineRenderer>().SetPosition(i, target.transform.position);
            i++;
        }

        StartCoroutine(Co_DisableLineRendereAfterDelay(lineRenderer, lifetime, vertexCount));
    }

    private void DealDamage(GameObject Target, float damage)
    {
        //Debug.Log("Dealing " + damage + " points of damage");
        //Set future health to prevent overkill
        IHealth enemyHealth = (IHealth)Target.GetComponent<BaseEnemy>();
        enemyHealth.futureHealth -= damage;

        //Damage target instantly
        IDamage enemyDealDamage = (IDamage)Target.GetComponent<BaseEnemy>();
        enemyDealDamage.damage = damage;
    }

   
    private GameObject SetNextTarget(GameObject originObj)
    {
        jumpEnemyList = base.GetEnemisInRange(originObj.transform.position, _jumpDistance); // Get all enemies in range.
        jumpEnemyList.RemoveAll(i => IHaveBeenHit.Contains(i)); // Remove enemies that have been hit

        //Targeting script
        Targeting scriptTargeting = GetComponent<Targeting>();
        currentTarget = scriptTargeting.ChooseTargetScanType(jumpEnemyList, targetSwitch);
        return currentTarget;
    }

    IEnumerator Co_DisableLineRendereAfterDelay(LineRenderer linerenderer, float lifetime, int vertexCount)
    {
        yield return new WaitForSeconds(lifetime);

        for (int i = 0; i < vertexCount; i++)
        {
            linerenderer.GetComponent<LineRenderer>().SetPosition(i, Vector3.zero);
        }
    }

    protected override void SetProjectileDamage(GameObject projectile)
    {
        // not relevant in this class as LightninWeapon do not use projectiles.
    }

    private bool PeformJump()
    {
        bool jump = false;
        if (_JumpChance > 100) // prevent out of range in Random below.
        {
            _JumpChance = 100;
        }

        //Crit
        if (_JumpChance != 0) //If we can jump
        {
            int i = UnityEngine.Random.Range(0, 100);// Pick a random number from 0 to 100

            if (i < _JumpChance)
            { // if i = 0 Peform jump.
                jump = true;
            }
        }
        return jump;
    }
    #endregion
}

