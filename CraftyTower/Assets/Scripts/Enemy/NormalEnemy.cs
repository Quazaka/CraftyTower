using UnityEngine;
using System.Collections;
using System;

public class NormalEnemy : BaseEnemy {

    protected override void Start()
    {
        base.Start();

        //Enemy stats
        MoveSpeed = 2;
        AttackDamage = 1; //CalculateDamage(0.1f, Wave.level);
        AttackRate = 1;
        DamageReduction = 0;
        health = CalculateHealth(1.5f, Wave.level);
        futureHealth = health;

        OnSpawn();
    }
}
