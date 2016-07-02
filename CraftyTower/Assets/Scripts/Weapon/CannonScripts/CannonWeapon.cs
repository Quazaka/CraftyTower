﻿using UnityEngine;
using System.Collections;
using System;

public class CannonWeapon : BaseWeapon
{
    public override float cooldown
    {
        get{ return 2.0f; }
    }

    public override float range
    {
        get{ return 15.0f; }
    }

    //Get damage
    protected override float GetProjectileDamage(GameObject projectile)
    {
        return projectile.GetComponent<CannonProjectile>().damage;
    }

    //set target
    protected override GameObject setTarget(GameObject projectile, GameObject currentTarget)
    {
        projectile.GetComponent<CannonProjectile>().target = currentTarget.transform; 

        return projectile;
    }
}
