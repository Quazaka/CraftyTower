using UnityEngine;
using System.Collections;
using System;

public class ArrowWeapon : BaseWeapon {


    public override float cooldown
    {
        get{ return 0.5f; }
    }

    public override float range
    {
        get { return 10.0f; }
    }

    //Get damage
    protected override float GetProjectileDamage(GameObject projectile)
    {
        return projectile.GetComponent<ArrowProjectile>().damage;
    }

    //Set Target
    protected override GameObject setTarget(GameObject projectile, GameObject currentTarget)
    {
        projectile.GetComponent<ArrowProjectile>().target = currentTarget.transform; //set target

        return projectile;
    }
}
