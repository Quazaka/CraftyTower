using System;
using System.Collections.Generic;
using UnityEngine;
using CraftyTower.Upgrades;

class UpgradeArrow : UpgradeBase
{

    //Arrow stats
    private float damage;
    private float range;
    private float firerate;
    private float critChance;
    private float critDamage;
    private float bonussDamageToBoss;
    private float PoiDoTDmg;
    private float PoiDuration;
    private float PoiReducedArmor;

    //Used as start
    protected override void ChildStart()
    {
        //Fetch weapon
        //weaponObj = GameObject.FindGameObjectWithTag("ArrowWeapon");
        //ArrowWeapon weapon = weaponObj.GetComponent(typeof(ArrowWeapon)) as ArrowWeapon;

        //Set tower type
        base.type = TowerType.ArrowTower;

    }

    //Excluding poison upgrade
    protected override void RollSmallSet()
    {
        /*foreach (float stat in arrowStats)
        {
            Debug.Log(stat.ToString());
        }*/
    }

    //Including poison upgrade
    protected override void RollLargeSet()
    {
        
    }

    //Write a uniqe key used for stacking in inventory
    protected override void CalcChecksum()
    {
        string checkSum = type.ToString()
            + damage.ToString()
            + range.ToString()
            + firerate.ToString()
            + critChance.ToString()
            + critDamage.ToString()
            + bonussDamageToBoss.ToString()
            + PoiDoTDmg.ToString()
            + PoiDuration.ToString()
            + PoiReducedArmor.ToString();
    }

}
