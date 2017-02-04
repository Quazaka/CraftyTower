using UnityEngine;
using System.Collections;
using System;

public class CannonWeapon : BaseWeapon
{
    //Projectile
    public GameObject projectilePrefab;
    private CannonProjectile currentProjectile;

    #region Upgrades
    private float _damage = 1;
    private float _tempDamage; //Store damage with modifiers
    private float _range = 5;
    private float _firerate = 1;
    private float _splashRadius = 1;
    //multiplyers: 1f = 100% damage, 1.1f = 110% damage, 0.5f = 50% damage
    private float _splashDamageMultiplier = 0.1f;
    private float _damageToNormalEnemyMultiplier = 1; 
    private float _damageToFastEnemyMultiplier = 1;
    private bool _haveBurningEffect = false;
    private float _burnDuration = 0;
    private float _burnRadius = 0;
    private float _burnDamagePerSec = 0;

    #region get/set
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    public override float Range
    {
        get{ return _range; }
        set { _range = value; }
    }

    public override float Firerate
    {
        get{ return _firerate; }
        set { _firerate = value; }
    }

    public float SplashRadius
    {
        get { return _splashRadius; }
        set { _splashRadius = value; }
    }

    public float SplashRadiusMultiplier
    {
        get { return _splashDamageMultiplier; }
        set { _splashDamageMultiplier = value; }
    }

    public float DamageToNormalEnemyMultiplier
    {
        get { return _damageToNormalEnemyMultiplier; }
        set { _damageToNormalEnemyMultiplier = value; }
    }

    public float DamageToFastEnemyMultiplyer
    {
        get { return _damageToFastEnemyMultiplier; }
        set { _damageToFastEnemyMultiplier = value; }
    }

    // TODO Implement burning effect in cannon weapon.
    public bool HaveBurningEffect 
    {
        get { return _haveBurningEffect; }
        set { _haveBurningEffect = value; }
    }

    public float BurnDuration
    {
        get { return _burnDuration; }
        set {  _burnDuration = value; }
    }

    public float BurnRadius
    {
        get { return _burnRadius; }
        set { _burnRadius = value; }
    }

        public float BurnDamagePerSec
    {
        get { return _burnDamagePerSec; }
        set { _burnDamagePerSec = value; }
    }
    #endregion

    //Calculate damage based on variables
    protected override float CalculateDamageWithVariables()
    {
        currentProjectile.Damage = _damage;
        currentProjectile.SplashRadius = _splashRadius;
        currentProjectile.SplashDamage *= _splashDamageMultiplier;

        if(currentTarget.tag == "FastEnemy")
        {
            currentProjectile.Damage *= _damageToFastEnemyMultiplier;
        }
        else if(currentTarget.tag == "NormalEnemy")
        {
            currentProjectile.Damage *= _damageToNormalEnemyMultiplier;
        }

        return currentProjectile.Damage;
    }
    #endregion

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
            currentProjectile.Damage = _damage;
            currentProjectile.SplashRadius = _splashRadius;
            currentProjectile.SplashDamage *= _splashDamageMultiplier;
        }

        PreventMultipleProjectiles(currentProjectile);
    }
}
