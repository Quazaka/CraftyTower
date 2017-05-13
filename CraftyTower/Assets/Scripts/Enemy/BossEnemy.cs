using UnityEngine;
using System.Collections;
using System;

public class BossEnemy : BaseEnemy {

    protected override void Start()
    {
        base.Start();

        //Enemy stats
        MoveSpeed = 0.2f;
        AttackDamage = CalculateDamage(1, Wave.level);
        AttackRate = 1;
        DamageReduction = 0;        
        health = CalculateHealth(20f, Wave.level);
        futureHealth = health;

        OnSpawn();
    }

    protected override int CalculateHealth(float health, int wave)
    {
        return (int)health * wave;
    }
}
