using UnityEngine;
using System.Collections;
using System;

public class ArrowWeapon : BaseWeapon {

    //Projectile
    public GameObject projectilePrefab;
    private ArrowProjectile currentProjectile;

    #region Upgrade variables
    private float _damage = 1;
    private float _firerate = 1;
    private float _range = 5;
    private float _critChance = 50;
    private float _critMultiplier = 10; // 1 = 100% damage 1.1 = 110% damage on crit
    private float _damageToBossMultiplier = 1; // 1 = 100% damage 1.1 = 110% damage to boss
    private bool _havePoisionEffect = false;
    private float _poisonDuration = 0;
    private float _poisonDamagePerSec = 0;

    #region get/set
    //public float Damage
    //{
    //    get { return _damage; }
    //    set { _damage = value; }
    //}

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

    public float CritChance
    {
        get { return _critChance; }
        set { _critChance = value; }
    }

    public float CritMultiplier
    {
        get { return _critMultiplier; }
        set { _critMultiplier = value; }
    }

    public float BonusDamageToBoss
    {
        get { return _damageToBossMultiplier; }
        set { _damageToBossMultiplier = value; }
    }

    // TODO Implement Poison on Arrow Weapon
    public bool HavePoisonEffect
    {
        get { return _havePoisionEffect; }
        set { _havePoisionEffect = value; }
    }

    public float PoisonDuration
    {
        get { return _poisonDuration; }
        set { _poisonDuration = value; }
    }

        public float PoisonDamagePerSec
    {
        get { return _poisonDamagePerSec; }
        set { _poisonDamagePerSec = value; }
    }
    #endregion

    //Calculate damage based on variables
    protected override float CalculateDamageWithVariables()
    {
        currentProjectile.Damage = _damage;

        //Crit
        if (_critChance != 0) //If we can crit
        {
            int i = UnityEngine.Random.Range(0, 100);// Pick a random number from 0 to 100

            if (i < _critChance) {
                //modifiedDamage *= _critMultiplier;
                currentProjectile.Damage *= _critMultiplier;
            }
        }

        //Boss bonus damage
        if (currentTarget.tag == "Boss")
        {
            currentProjectile.Damage *= _damageToBossMultiplier;
        }

        Debug.Log("modified arrowprojectile dmg: " + currentProjectile.Damage);
        return currentProjectile.Damage;
    }
    #endregion

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
            currentProjectile.Damage = _damage;
        }

        PreventMultipleProjectiles(currentProjectile);
    }
}

