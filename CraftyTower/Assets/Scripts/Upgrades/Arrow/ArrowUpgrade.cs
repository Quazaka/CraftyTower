using System;
using UnityEngine;
using CraftyTower.Upgrades;

public class ArrowUpgrade : BaseUpgrade
{
    //Arrow stats
    private float damage = 0;
    private float range = 0;
    private float firerate = 0;
    private float critChance = 0;
    private float critDamage = 0;
    private float bonussDamageToBoss = 0;

    private float PoiDoTDmg = 0;
    private float PoiDuration = 0;
    private float PoiReducedArmor = 0;

    //Used as start
    protected override void ChildStart()
    {
        
        //Set tower type
        //base.room = ArrowRoom;

    }

    //Excluding poison upgrade
    protected override void RollSmallSet()
    {

    }

    //Including poison upgrade
    protected override void RollLargeSet()
    {
        
    }

    //Write a uniqe key used for stacking in inventory
    protected override void CalcChecksum()
    {
        string checkSum = room.ToString()
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
