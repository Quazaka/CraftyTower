using UnityEngine;
using System.Collections;
using System;

public class ArrowBase : WeaponBase {


    public override float cooldown
    {
        get{ return 0.5f; }
    }

    public override float range
    {
        get { return 10.0f; }
    }

    //Arrow implementation of shoot
    protected override void Shoot(GameObject currentTarget)
    {
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity); //create projectile

        if (currentTarget == null)
        {
            base.RemoveNullObjectFromList(enemyList); //Remove null objects from target list
            return;
        }

        projectile.GetComponent<ArrowProjectile>().target = currentTarget.transform; //set target

        //Set future health to prevent overkill
        //IHealth enemyHealth = currentTarget.GetComponent<Enemy>();
        //enemyHealth.futureHealth -= projectile.GetComponent<ArrowProjectile>().damage;
    }

}
