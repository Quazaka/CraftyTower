using UnityEngine;
using System.Collections;

public class FastEnemy : BaseEnemy {

    protected override void Start()
    {
        base.Start();

        //Enemy stats
        MoveSpeed = 2;
        AttackDamage = CalculateDamage(0.05f, Wave.level);
        AttackRate = 1;
        DamageReduction = 0;        
        health = CalculateHealth(1.3f, Wave.level);
        futureHealth = health;

        OnSpawn();
    }
}
