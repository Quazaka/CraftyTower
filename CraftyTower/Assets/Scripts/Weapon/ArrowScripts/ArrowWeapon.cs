using UnityEngine;
using System.Collections;
using System;

public class ArrowWeapon : BaseWeapon {

    #region Upgrade variables
    private float _damage = 1;
    private float _tempDamage; // Store damage with modifiers
    private float _firerate = 1;
    private float _range = 5;
    private float _critChance = 50;
    private float _critMultiplier = 10; // 1 = 100% damage 1.1 = 110% damage on crit
    private float _damageToBossMultiplier = 1; // 1 = 100% damage 1.1 = 110% damage to boss
    private bool _havePoisionEffect = false;
    private float _poisonDuration = 0;
    private float _poisonDamagePerSec = 0;

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
        float tempDamage = _damage;

        //Crit
        if (_critChance != 0) //If we can crit
        {
            int i = UnityEngine.Random.Range(0, 100);// Pick a random number from 0 to 100

            if (i < _critChance) {
                tempDamage *= _critMultiplier;
            }
        }

        //Boss bonus damage
        if (currentTarget.tag == "Boss")
        {
            tempDamage *= _damageToBossMultiplier;
        }

        _tempDamage = tempDamage;
        return tempDamage;
    }
    #endregion

    //Set Target
    protected override GameObject setTarget(GameObject projectile, GameObject currentTarget)
    {
        projectile.GetComponent<ArrowProjectile>().target = currentTarget.transform; //set target

        return projectile;
    }

    //Set damage on projectile
    protected override void SetProjectileDamage(GameObject projectile)
    {
        projectile.GetComponent<ArrowProjectile>().Damage = _tempDamage; 
    }
}

