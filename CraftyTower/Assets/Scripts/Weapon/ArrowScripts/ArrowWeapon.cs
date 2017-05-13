using UnityEngine;
using System.Collections;
using System;

public class ArrowWeapon : BaseWeapon {

    //Projectile
    public GameObject projectilePrefab;
    private ArrowProjectile currentProjectile;

    #region Unique Arrow Variables
    private float critChance;
    private float critMultiplier; // 1 = 100% damage 1.1 = 110% damage on crit
    private float damageToBossMultiplier; // 1 = 100% damage 1.1 = 110% damage to boss
    #endregion

    protected override void Start()
    {
        // Base Variables
        Damage = 1f;
        Firerate = 1f;
        Range = 5f;
        // Unique modifiers
        critChance = 50f;
        critMultiplier = 10f;
        damageToBossMultiplier = 1f;

        base.Start();
    }

    //Ready up the weapon, calculate projectile damage, set its target and parent.
    //bool used to determine whether to use modifiers such as crit etc.
    protected override void ReadyWeapon(GameObject target, bool modifyDamage)
    {
        //Create projectile, set parent and target
        currentProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ArrowProjectile>();
        currentProjectile.transform.parent = this.gameObject.transform;
        currentProjectile.target = target.transform;

        if (modifyDamage)
        {
            currentProjectile.Damage = CalculateDamageWithVariables();
        }
        else
        {
            currentProjectile.Damage = Damage;
        }

        PreventMultipleProjectiles(currentProjectile);
    }

    //Calculate damage based on variables
    protected override float CalculateDamageWithVariables()
    {
        currentProjectile.Damage = Damage;

        //Crit
        if (critChance != 0) //If we can crit
        {
            int i = UnityEngine.Random.Range(0, 100);// Pick a random number from 0 to 100

            if (i < critChance) {
                //modifiedDamage *= _critMultiplier;
                currentProjectile.Damage *= critMultiplier;
            }
        }

        //Boss bonus damage
        if (currentTarget.tag == "Boss")
        {
            currentProjectile.Damage *= damageToBossMultiplier;
        }

        Debug.Log("modified arrowprojectile dmg: " + currentProjectile.Damage);
        return currentProjectile.Damage;
    }
}

