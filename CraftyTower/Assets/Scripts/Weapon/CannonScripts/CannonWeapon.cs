using UnityEngine;
using System.Collections;
using System;

public class CannonWeapon : BaseWeapon
{
    //Projectile
    public GameObject projectilePrefab;
    private CannonProjectile currentProjectile;

    #region Unique Cannon Variables
    private float splashRadius;
    private float splashDamageMultiplier;
    #endregion

    protected override void Start()
    {
        // Base Variables
        Damage = 1f;
        Range = 5f;
        Firerate = 1f;
        // Unique modifiers
        splashRadius = 1f;
        splashDamageMultiplier = 0.1f;

        base.Start();
    }

    //Ready up the weapon, calculate projectile damage, set its target and parent.
    //bool used to determine whether to use modifiers such as splashdamage etc.
    protected override void ReadyWeapon(GameObject target, bool modifyDamage)
    {
        //Create projectile, set parent and target
        currentProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<CannonProjectile>();        
        currentProjectile.transform.parent = this.gameObject.transform;
        currentProjectile.target = target.transform;

        if (modifyDamage)
        {
            currentProjectile.Damage = CalculateDamageWithVariables();
        }
        else
        {
            currentProjectile.Damage = Damage;
            currentProjectile.SplashRadius = splashRadius;
            currentProjectile.SplashDamage *= splashDamageMultiplier;
        }

        PreventMultipleProjectiles(currentProjectile);
    }

    //Calculate damage based on variables
    protected override float CalculateDamageWithVariables()
    {
        currentProjectile.Damage = Damage;
        currentProjectile.SplashRadius = splashRadius;
        currentProjectile.SplashDamage *= splashDamageMultiplier;

        if (currentTarget.tag == "FastEnemy")
        {
            currentProjectile.Damage *= 1.3f;
        }
        else if (currentTarget.tag == "NormalEnemy")
        {
            currentProjectile.Damage *= 1.1f;
        }

        return currentProjectile.Damage;
    }
}
