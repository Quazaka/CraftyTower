using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class LightningWeapon : BaseWeapon {

    private List<GameObject> targetsInArcRange = new List<GameObject>(); //List to store enemies that can be arced to
    private List<GameObject> targets = new List<GameObject>(); //List to store enemies that should be hit by attack - arcchain
    private float lifetime = 0.2f; //How long the arc should be displayed
    private float _arcDamage; //Store damage during arcs

    #region Upgrades
    [Range (0f, 100f)]
    [SerializeField]
    private float _damage = 1; //private constant as damage
    private float _firerate = 1;
    private float _range = 5;
    private float _arcChance = 50; //Chance that an attack will arc - between 0 & 100.
    private float _damageLossPerArc = 0.5f; // Damage loss per arc - 0.5f = 50% damage loss
    private int _arcCount = 5; //Number of times the lightning should arc
    private float _arcDistance = 10.5f; //Max distance between arcs
    private bool _haveRodEffect = false;
    private float _rodChance = 0;
    private float _rodDuration = 0;
    private float _rodCooldown = 0;
    private float _rodDamage = 0;
    #endregion

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

    public float ArcChance
    {
        get{ return _arcChance; }
        set{ _arcChance = value; }
    }

    public float DamageLossPerArc
    {
        get{ return _damageLossPerArc; }
        set{ _damageLossPerArc = value; }
    }

    public int ArcCount
    {
        get{ return _arcCount; }
        set{ _arcCount = value; }
    }

    public float ArcDistance
    {
        get{ return _arcDistance; }
        set{ _arcDistance = value; }
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

    void Awake() // This is used instead of Start(), as start runs in the baseclass.
    {
        targetSwitch = 1; //Closest target
    }

    #region upgrades
    //Calculate damage based on variables
    protected override float CalculateDamageWithVariables()
    {
        return _damage;
    }
    #endregion

    //Ready up the weapon and attack, calculating damage and finding targets
    protected override void ReadyWeapon(GameObject target, bool modifyDamage)
    {
        Debug.Log("Shooting with lightning");
        //Remove null targets from enemyList
        RemoveNullObjectFromList(enemyList);       

        if (modifyDamage)
        {
            _arcDamage = CalculateDamageWithVariables();
        }
        else
        {
            _arcDamage = _damage;
        }

        SetTargets(target);
        Attack(); 
    }

    //Gets the targets that the attack will arc to based on first target
    private void SetTargets(GameObject originTarget)
    {
        targets.Add(originTarget);
        //If PeformArc returns true, find all arctarget based on targeting script
        if (PerformArc())
        {
            Targeting scriptTargeting = GetComponent<Targeting>();
            GameObject nextTarget = originTarget;

            for (int i = 0; i < _arcCount; i++)
            {
                Debug.Log("Arc #" + i);
                targetsInArcRange = base.GetEnemiesInRange(nextTarget.transform.position, _arcDistance); //Get all enemies in range of a target
                targetsInArcRange.RemoveAll(j => targets.Contains(j)); //Remove targets that are already in the arc chain

                //Use targeting script to determine what enemy in range to add to list of targets
                nextTarget = scriptTargeting.ChooseTargetScanType(targetsInArcRange, targetSwitch);
                if (nextTarget == null) { break; }
                targets.Add(nextTarget); //Add target to list of targets to be hit by arc chain
                targetsInArcRange.Clear(); //Clear the list for next iteration
            }
        }
    }

    //Activate the lightning attack - drawing the arc chain and dealing damage
    private void Attack()
    {
        //First draw the attack using linerender
        LineRenderer arcChain = gameObject.GetComponent<LineRenderer>();

        //How many parts to divide the line into
        int arcPoints = targets.Count() + 1;        
        arcChain.numPositions = arcPoints;

        //Set width of lightning and start position
        arcChain.startWidth = 0.25f;
        arcChain.endWidth = 0.25f;     
        arcChain.SetPosition(0, gameObject.transform.position); //Start position of the attack - for now it's the weaponbase

        //Set the positions of each of the targets in the arc chain
        int i = 1;
        foreach (GameObject target in targets)
        {
            arcChain.SetPosition(i, target.transform.position);
            i++;
        }

        arcChain.enabled = true; //Enable line renderer

        DealDamage();
        StartCoroutine(Co_DisableLineRendereAfterDelay(arcChain, lifetime, arcPoints));
        targets.Clear(); //Clear the list of targets for next attack
    }

    //Handles dealing damage to enemies affected by the attack
    private void DealDamage()
    {
        int i = 0;

        foreach (GameObject target in targets)
        {
            if (i > 0) // if the attacks arcs - reduce the damage done (i == 0 is first target and should do max damage)
            {
                _arcDamage *= _damageLossPerArc;
            }
            PreventMultipleAttacks(target, _arcDamage);
            i++;
        }

        _arcDamage = _damage; //Reset the _arcDamage to equal the base _damage so each unique chain does equal damage.
    }

    // Determines whether the attack should arc or not
    private bool PerformArc()
    {
        //If we can arc
        if (_arcChance > 0)
        {
            // prevent out of range in Random below.
            if (_arcChance > 100) { _arcChance = 100; }

            // Pick a random number from 0 to 100
            int i = UnityEngine.Random.Range(0, 100);

            // if i is smaller than arcchance then peform an arc.
            if (i <= _arcChance) { return true; }
        }
        return false;
    }

    IEnumerator Co_DisableLineRendereAfterDelay(LineRenderer linerenderer, float lifetime, int vertexCount)
    {
        yield return new WaitForSeconds(lifetime);

        for (int i = 0; i < vertexCount; i++)
        {
            linerenderer.SetPosition(i, Vector3.zero);
        }
    }
}

