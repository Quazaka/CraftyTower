using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class LightningWeapon : BaseWeapon {

    private List<GameObject> targetsInArcRange = new List<GameObject>(); //List to store enemies that can be arced to
    private List<GameObject> targets = new List<GameObject>(); //List to store enemies that should be hit by attack - arcchain

    #region Unique Lightning Variables
    [Range (0f, 100f)]
    private float arcDamage; // Store damage during arcs
    private float arcChance; // Chance that an attack will arc - between 0 & 100.
    private float damageLossPerArc; // Damage loss per arc - 0.5f = 50% damage loss
    private int arcCount; // Number of times the lightning should arc
    private float arcDistance; // Max distance between arcs
    private float lifetime; // How long the arc should be displayed

    // TODO: Implement Rod effect - Not sure what this means (guessing constant lightning on one enemy)
    //private bool _haveRodEffect = false;
    //private float _rodChance = 0;
    //private float _rodDuration = 0;
    //private float _rodCooldown = 0;
    //private float _rodDamage = 0;
    #endregion
    
    protected override void Start()
    {
        // Base Variables
        Damage = 1f;
        Firerate = 1f;
        Range = 5f;
        // Unique modifiers
        arcChance = 50f;
        damageLossPerArc = 0.5f;
        arcCount = 5;
        arcDistance = 10.5f;
        lifetime = 0.2f;

        base.Start();
    }
    
    //Ready up the weapon and attack, calculating damage and finding targets
    protected override void ReadyWeapon(GameObject target, bool modifyDamage)
    {
        Debug.Log("Shooting with lightning");
        //Remove null targets from enemyList
        RemoveNullObjectFromList(enemyList);       

        if (modifyDamage)
        {
            arcDamage = CalculateDamageWithVariables();
        }
        else
        {
            arcDamage = Damage;
        }

        SetTargets(target);
        Attack(); 
    }

    //Calculate damage based on variables
    protected override float CalculateDamageWithVariables()
    {
        return Damage;
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

            for (int i = 0; i < arcCount; i++)
            {
                Debug.Log("Arc #" + i);
                targetsInArcRange = base.GetEnemiesInRange(nextTarget.transform.position, arcDistance); //Get all enemies in range of a target
                targetsInArcRange.RemoveAll(j => targets.Contains(j)); //Remove targets that are already in the arc chain

                //Use targeting script to determine what enemy in range to add to list of targets
                nextTarget = scriptTargeting.ChooseTargetScanType(targetsInArcRange, targetingType);
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
                arcDamage *= damageLossPerArc;
            }
            PreventMultipleAttacks(target, arcDamage);
            i++;
        }

        arcDamage = Damage; //Reset the _arcDamage to equal the base _damage so each unique chain does equal damage.
    }

    // Determines whether the attack should arc or not
    private bool PerformArc()
    {
        //If we can arc
        if (arcChance > 0)
        {
            // prevent out of range in Random below.
            if (arcChance > 100) { arcChance = 100; }

            // Pick a random number from 0 to 100
            int i = UnityEngine.Random.Range(0, 100);

            // if i is smaller than arcchance then peform an arc.
            if (i <= arcChance) { return true; }
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

